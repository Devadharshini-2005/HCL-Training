using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class JobOpening
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string JobDescription { get; set; } = string.Empty;

    public string RequiredSkills { get; set; } = string.Empty;

    public string JobLocation { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public JobStatus Status { get; set; } = JobStatus.Draft;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property

    public Company Company { get; set; } = null!;


    public ICollection<JobTargetCollege> TargetColleges { get; set; }
        = new List<JobTargetCollege>();

    public ICollection<JobEligibleDepartment> EligibleDepartments { get; set; }
        = new List<JobEligibleDepartment>();

    public ICollection<JobEligibleGraduationYear> EligibleGraduationYears { get; set; }
        = new List<JobEligibleGraduationYear>();

}