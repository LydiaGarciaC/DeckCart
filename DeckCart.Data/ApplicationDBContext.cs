using Microsoft.EntityFrameworkCore;
using System.Reflection;
using DeckCart.Data.Entities;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<CartEntity> Carts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed of items to apply on initial migration
        modelBuilder.Entity<ItemEntity>().HasData(
            new ItemEntity { Id = 1, Name = "Item 1", Price = 1.11m },
            new ItemEntity { Id = 2, Name = "Item 2", Price = 2.22m }
        );

        modelBuilder.Entity<UserEntity>().HasData(
            new UserEntity { Id = 1, Name = "User 1" },
            new UserEntity { Id = 2, Name = "User 2" }
        );
    }
}