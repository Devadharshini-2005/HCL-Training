
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Jobs;

public class UpdateJobRequestDto
{
    [Required(ErrorMessage = "Job title is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Job title must contain 2 to 200 characters.")]
    public string JobTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required.")]
    [StringLength(5000, MinimumLength = 10,
        ErrorMessage = "Job description must contain 10 to 5000 characters.")]
    public string JobDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required skills are required.")]
    [StringLength(2000, MinimumLength = 2,
        ErrorMessage = "Required skills must contain 2 to 2000 characters.")]
    public string RequiredSkills { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job location is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Job location must contain 2 to 200 characters.")]
    public string JobLocation { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "100000000",
        ErrorMessage = "Salary must be between 0 and 100000000.")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Application deadline is required.")]
    public DateTime ApplicationDeadline { get; set; }

    [Required(ErrorMessage = "Select at least one target college.")]
    [MinLength(1, ErrorMessage = "Select at least one target college.")]
    public List<int> TargetCollegeIds { get; set; } = new();

    [Required(ErrorMessage = "Select at least one eligible department.")]
    [MinLength(1, ErrorMessage = "Select at least one eligible department.")]
    public List<string> EligibleDepartments { get; set; } = new();

    [Required(ErrorMessage = "Select at least one graduation year.")]
    [MinLength(1, ErrorMessage = "Select at least one graduation year.")]
    public List<int> EligibleGraduationYears { get; set; } = new();
}
