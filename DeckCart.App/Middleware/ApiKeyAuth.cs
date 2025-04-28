using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DeckCart.App.Middleware
{
    public class ApiKeyAuth
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "X-API-KEY";
        private readonly string _apiKey;

        public ApiKeyAuth(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKey"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var requestApiKey))
            {
                Log.Warning("API key header '{ApiKeyHeaderName}' is missing", APIKEYNAME);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Api key is missing");
                return;
            }

            if (requestApiKey != _apiKey)
            {
                Log.Error("Api key provided '{requestApiKey}' is invalid", requestApiKey);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Api key is invalid");                
                return;
            }

            await _next(context);
        }
    }

}
