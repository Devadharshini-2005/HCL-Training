
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Jobs;

public class JobResponseDto
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string JobDescription { get; set; } = string.Empty;

    public string RequiredSkills { get; set; } = string.Empty;

    public string JobLocation { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public JobStatus Status { get; set; }

    public List<int> TargetCollegeIds { get; set; } = new();

    public List<string> EligibleDepartments { get; set; } = new();

    public List<int> EligibleGraduationYears { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
