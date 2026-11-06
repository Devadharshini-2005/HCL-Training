
using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations;

public class JobApplicationConfiguration
    : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        // Table name
        builder.ToTable("JobApplications");

        // Primary key
        builder.HasKey(a => a.Id);

        // Application reference
        builder.Property(a => a.ApplicationReference)
            .IsRequired()
            .HasMaxLength(50);

        // Application status
        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>();

        // Applied date
        builder.Property(a => a.AppliedAt)
            .IsRequired();

        // Unique application reference
        builder.HasIndex(a => a.ApplicationReference)
            .IsUnique();

        // Prevent duplicate applications for the same job
        builder.HasIndex(a => new { a.StudentId, a.JobOpeningId })
            .IsUnique();

        // Relationship with Student
        // One Student can have many JobApplications
        builder.HasOne(a => a.Student)
            .WithMany(s => s.JobApplications)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with JobOpening
        // One JobOpening can have many JobApplications
        builder.HasOne(a => a.JobOpening)
            .WithMany()
            .HasForeignKey(a => a.JobOpeningId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
