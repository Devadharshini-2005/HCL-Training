
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Students;

public class CreateStudentRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Password must contain 8 to 100 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Register number is required.")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Register number must contain 2 to 50 characters.")]
    public string RegisterNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Department must contain 2 to 100 characters.")]
    public string Department { get; set; } = string.Empty;

    [Range(2020, 2100,
        ErrorMessage = "Enter a valid graduation year.")]
    public int GraduationYear { get; set; }

    [Url(ErrorMessage = "Enter a valid resume URL.")]
    [StringLength(500)]
    public string? ResumeUrl { get; set; }
}
