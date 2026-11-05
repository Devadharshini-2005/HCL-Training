using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // College and Company

    public DbSet<College> Colleges { get; set; }

    public DbSet<PlacementOfficer> PlacementOfficers { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<CompanyHR> CompanyHRs { get; set; }

    // Student

    public DbSet<Student> Students { get; set; }

    // Job

    public DbSet<JobOpening> JobOpenings { get; set; }

    public DbSet<JobTargetCollege> JobTargetColleges { get; set; }

    public DbSet<JobEligibleDepartment> JobEligibleDepartments { get; set; }

    public DbSet<JobEligibleGraduationYear> JobEligibleGraduationYears { get; set; }

    // Applications

    public DbSet<JobApplication> JobApplications { get; set; }

    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }

    // Recruitment and Interviews

    public DbSet<RecruitmentDrive> RecruitmentDrives { get; set; }

    public DbSet<InterviewRound> InterviewRounds { get; set; }

    public DbSet<InterviewSchedule> InterviewSchedules { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}