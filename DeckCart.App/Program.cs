using DeckCart.Business.Handlers;
using DeckCart.Business.Handlers.Interfaces;
using DeckCart.Data.Repositories.Interfaces;
using DeckCart.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using DeckCart.App.Middleware;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = Environment.GetEnvironmentVariable("DECKCART_CONNECTION_STRING");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Configuration.AddJsonFile("appsettings.json");

    //Adding serilog based on https://medium.com/@brucycenteio/adding-serilog-to-asp-net-core-net-7-8-5cba1d0dea2
    builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

    //Adding auth to Swagger based on https://medium.com/@meghnav274/adding-authorization-option-in-swagger-638abfb0041f
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "DeckCart",
            Version = "v1"
        });
        c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
        {
            Name = "X-API-KEY",
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Description = "ApiKey is required to authenticate",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
    });

    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    // Conrollers
    builder.Services.AddControllers();

    // Handlers
    builder.Services.AddScoped<ICartHandler, CartHandler>();

    //Repos
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ICartRepository, CartItemRepository>();
    builder.Services.AddScoped<IItemRepository, ItemRepository>();    

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty; // Enabling Swagger on root
        });
    }

    app.UseRouting();

    app.MapControllers();

    //We don't require ApiKey when accessing the swagger pages
    app.UseWhen(context => !context.Request.Path.StartsWithSegments("/swagger"), builder =>
    {
        builder.UseMiddleware<ApiKeyAuth>();
    });
    app.UseAuthorization();


    app.Run();
}
catch (Exception ex)
{    
    Log.Fatal(ex, "DeckCart failed on startup");
}
finally
{
    Log.CloseAndFlush();
}

