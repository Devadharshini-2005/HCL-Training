
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.CompanyHRs;

public class CreateCompanyHRRequestDto
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

    [Required(ErrorMessage = "Company ID is required.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "Please select a valid company.")]
    public int CompanyId { get; set; }
}
