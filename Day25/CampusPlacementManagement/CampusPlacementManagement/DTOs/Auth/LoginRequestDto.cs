
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public string Password { get; set; } = string.Empty;
}
