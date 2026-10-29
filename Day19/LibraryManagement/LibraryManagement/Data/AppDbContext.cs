using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            // Table name
            entity.ToTable("Books", table =>
            {
                table.HasCheckConstraint(
                    "CK_Books_Price",
                    "[Price] >= 0");

                table.HasCheckConstraint(
                    "CK_Books_TotalCopies",
                    "[TotalCopies] >= 0");

                table.HasCheckConstraint(
                    "CK_Books_AvailableCopies",
                    "[AvailableCopies] >= 0 AND [AvailableCopies] <= [TotalCopies]");
            });

            // Primary key
            entity.HasKey(b => b.BookId);

            // Book ID
            entity.Property(b => b.BookId)
                  .ValueGeneratedOnAdd();

            // Title
            entity.Property(b => b.Title)
                  .IsRequired()
                  .HasMaxLength(150);

            // Author
            entity.Property(b => b.Author)
                  .IsRequired()
                  .HasMaxLength(100);

            // ISBN
            entity.Property(b => b.ISBN)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasIndex(b => b.ISBN)
                  .IsUnique();

            // Price
            entity.Property(b => b.Price)
                  .HasPrecision(10, 2)
                  .IsRequired();

            // Total copies
            entity.Property(b => b.TotalCopies)
                  .IsRequired();

            // Available copies
            entity.Property(b => b.AvailableCopies)
                  .IsRequired();
        });
    }
}