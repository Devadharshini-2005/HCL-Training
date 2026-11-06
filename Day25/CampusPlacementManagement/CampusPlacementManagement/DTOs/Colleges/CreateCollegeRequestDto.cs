
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges;

public class CreateCollegeRequestDto
{
    [Required(ErrorMessage = "College name is required.")]
    [StringLength(200, MinimumLength = 2,
        ErrorMessage = "College name must contain 2 to 200 characters.")]
    public string CollegeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "College code is required.")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "College code must contain 2 to 50 characters.")]
    public string CollegeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "College address is required.")]
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
