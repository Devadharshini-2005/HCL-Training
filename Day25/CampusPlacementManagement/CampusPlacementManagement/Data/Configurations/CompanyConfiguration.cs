using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        // Table name
        builder.ToTable("Companies");

        // Primary key
        builder.HasKey(c => c.Id);

        // Company name
        builder.Property(c => c.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        // Description
        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        // Website
        builder.Property(c => c.Website)
            .HasMaxLength(250);

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

        // Company status
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        // Created date
        builder.Property(c => c.CreatedAt)
            .IsRequired();
    }
}