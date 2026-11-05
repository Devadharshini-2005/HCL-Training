using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Primary key
        builder.HasKey(rt => rt.Id);

        // Token hash must be unique
        builder.Property(rt => rt.TokenHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(rt => rt.TokenHash)
            .IsUnique();

        // User relationship
        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(rt => rt.UserId)
            .IsRequired();

        // Replaced token hash is optional
        builder.Property(rt => rt.ReplacedByTokenHash)
            .HasMaxLength(500);

        builder.Property(rt => rt.CreatedAt)
            .IsRequired();

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();
    }
}