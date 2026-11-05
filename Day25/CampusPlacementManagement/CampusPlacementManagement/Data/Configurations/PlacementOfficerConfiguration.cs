using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class PlacementOfficerConfiguration
    : IEntityTypeConfiguration<PlacementOfficer>
{
    public void Configure(EntityTypeBuilder<PlacementOfficer> builder)
    {
        // Table name
        builder.ToTable("PlacementOfficers");

        // Primary key
        builder.HasKey(po => po.Id);

        // User ID
        builder.Property(po => po.UserId)
            .IsRequired();

        // College ID
        builder.Property(po => po.CollegeId)
            .IsRequired();

        // Created date
        builder.Property(po => po.CreatedAt)
            .IsRequired();

        // Relationship with ApplicationUser
        builder.HasOne(po => po.User)
            .WithMany()
            .HasForeignKey(po => po.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with College
        builder.HasOne(po => po.College)
            .WithMany()
            .HasForeignKey(po => po.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent one user from having duplicate officer profiles
        builder.HasIndex(po => po.UserId)
            .IsUnique();
    }
}