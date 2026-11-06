
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "Refresh token is required.")]
    [StringLength(
        500,
        MinimumLength = 20,
        ErrorMessage = "Refresh token must be between 20 and 500 characters.")]
    public string RefreshToken { get; set; } = string.Empty;
}
