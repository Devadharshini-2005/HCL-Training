using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Table name
        builder.ToTable("Students");

        // Primary key
        builder.HasKey(s => s.Id);

        // User ID
        builder.Property(s => s.UserId)
            .IsRequired();

        // Register number
        builder.Property(s => s.RegisterNumber)
            .IsRequired()
            .HasMaxLength(50);

        // Department
        builder.Property(s => s.Department)
            .IsRequired()
            .HasMaxLength(100);

        // Graduation year
        builder.Property(s => s.GraduationYear)
            .IsRequired();

        // Resume URL
        builder.Property(s => s.ResumeUrl)
            .HasMaxLength(500);

        // Created date
        builder.Property(s => s.CreatedAt)
            .IsRequired();

        // One student profile per login account
        builder.HasIndex(s => s.UserId)
            .IsUnique();

        // Register number must be unique within a college
        builder.HasIndex(s => new { s.CollegeId, s.RegisterNumber })
            .IsUnique();

        // Relationship with ApplicationUser
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with College
        builder.HasOne(s => s.College)
            .WithMany()
            .HasForeignKey(s => s.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}