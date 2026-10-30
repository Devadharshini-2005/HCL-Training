using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Models;

namespace TrainingManagementSystem.Api.Data;

public class TrainingDbContext(DbContextOptions<TrainingDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<UserAccount> Users => Set<UserAccount>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasIndex(c => c.CourseCode).IsUnique();
        modelBuilder.Entity<UserAccount>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Course>().Property(c => c.Fee).HasPrecision(10, 2);
        modelBuilder.Entity<Enrollment>().Property(e => e.BaseFee).HasPrecision(10, 2);
        modelBuilder.Entity<Enrollment>().Property(e => e.DiscountAmount).HasPrecision(10, 2);
        modelBuilder.Entity<Enrollment>().Property(e => e.FinalFee).HasPrecision(10, 2);
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(10, 2);
    }
}
