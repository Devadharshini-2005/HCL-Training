
using FoodHub.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Data;

public class FoodHubDbContext : DbContext
{
    public FoodHubDbContext(DbContextOptions<FoodHubDbContext> options)
        : base(options)
    {
    }

    // Database Tables
    public DbSet<Category> Categories { get; set; }

    public DbSet<FoodItem> FoodItems { get; set; }

    public DbSet<FoodAvailability> FoodAvailabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category - FoodItem (One-to-Many)
        modelBuilder.Entity<FoodItem>()
            .HasOne(f => f.Category)
            .WithMany(c => c.FoodItems)
            .HasForeignKey(f => f.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // FoodItem - FoodAvailability (One-to-One)
        modelBuilder.Entity<FoodAvailability>()
            .HasOne(a => a.FoodItem)
            .WithOne()
            .HasForeignKey<FoodAvailability>(a => a.FoodItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // FoodItem Price Configuration
        modelBuilder.Entity<FoodItem>()
            .Property(f => f.Price)
            .HasPrecision(10, 2);
    }
}
