using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class JobOpeningConfiguration
    : IEntityTypeConfiguration<JobOpening>
{
    public void Configure(EntityTypeBuilder<JobOpening> builder)
    {
        // Table name
        builder.ToTable("JobOpenings");

        // Primary key
        builder.HasKey(j => j.Id);

        // Job title
        builder.Property(j => j.JobTitle)
            .IsRequired()
            .HasMaxLength(200);

        // Job description
        builder.Property(j => j.JobDescription)
            .IsRequired()
            .HasMaxLength(2000);

        // Required skills
        builder.Property(j => j.RequiredSkills)
            .IsRequired()
            .HasMaxLength(500);

        // Job location
        builder.Property(j => j.JobLocation)
            .IsRequired()
            .HasMaxLength(200);

        // Salary
        builder.Property(j => j.Salary)
            .HasPrecision(12, 2);

        // Application deadline
        builder.Property(j => j.ApplicationDeadline)
            .IsRequired();

        // Job status
        builder.Property(j => j.Status)
            .IsRequired()
            .HasConversion<int>();

        // Created date
        builder.Property(j => j.CreatedAt)
            .IsRequired();

        // Relationship with Company
        builder.HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}