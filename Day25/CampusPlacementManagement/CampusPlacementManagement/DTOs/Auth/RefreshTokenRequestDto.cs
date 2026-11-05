using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}