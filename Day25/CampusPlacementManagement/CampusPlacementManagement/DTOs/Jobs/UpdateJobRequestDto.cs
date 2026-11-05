
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Jobs;

public class UpdateJobRequestDto
{
    [Required]
    [StringLength(200)]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(5000)]
    public string JobDescription { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string RequiredSkills { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string JobLocation { get; set; } = string.Empty;

    [Range(0, 100000000)]
    public decimal Salary { get; set; }

    [Required]
    public DateTime ApplicationDeadline { get; set; }

    [Required]
    [MinLength(1)]
    public List<int> TargetCollegeIds { get; set; } = new();

    [Required]
    [MinLength(1)]
    public List<string> EligibleDepartments { get; set; } = new();

    [Required]
    [MinLength(1)]
    public List<int> EligibleGraduationYears { get; set; } = new();
}
