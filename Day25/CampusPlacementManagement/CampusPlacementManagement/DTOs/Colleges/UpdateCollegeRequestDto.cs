
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges;

public class UpdateCollegeRequestDto
{
    [Required(ErrorMessage = "College name is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "College name must be between 2 and 200 characters.")]
    public string CollegeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(300, MinimumLength = 5,
        ErrorMessage = "Address must be between 5 and 300 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "City must be between 2 and 100 characters.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "State must be between 2 and 100 characters.")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;
}
