
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Companies;

public class CreateCompanyRequestDto
{
    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "Company name must contain 2 to 200 characters.")]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(1000,
        ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Enter a valid website URL.")]
    [StringLength(500)]
    public string? Website { get; set; }

    [Required(ErrorMessage = "Company address is required.")]
    [StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
}
