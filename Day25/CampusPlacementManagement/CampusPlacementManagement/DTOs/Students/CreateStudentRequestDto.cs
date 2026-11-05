
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Students;

public class CreateStudentRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string RegisterNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required]
    [Range(2020, 2100)]
    public int GraduationYear { get; set; }

    [Url]
    public string? ResumeUrl { get; set; }
}
