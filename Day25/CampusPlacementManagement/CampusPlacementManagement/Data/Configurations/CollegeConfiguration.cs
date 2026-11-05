using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class CollegeConfiguration : IEntityTypeConfiguration<College>
{
    public void Configure(EntityTypeBuilder<College> builder)
    {
        // Table name
        builder.ToTable("Colleges");

        // Primary key
        builder.HasKey(c => c.Id);

        // College name
        builder.Property(c => c.CollegeName)
            .IsRequired()
            .HasMaxLength(200);

        // College code
        builder.Property(c => c.CollegeCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.CollegeCode)
            .IsUnique();

        // Address
        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.State)
            .IsRequired()
            .HasMaxLength(100);

        // Contact details
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        // College status
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        // Created date
        builder.Property(c => c.CreatedAt)
            .IsRequired();
    }
}