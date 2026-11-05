namespace CampusPlacementManagement.DTOs.Auth;

public class LoginResponseDto
{
    // JWT access token
    public string Token { get; set; } = string.Empty;

    // Refresh token
    public string RefreshToken { get; set; } = string.Empty;

    // Access token expiry time
    public DateTime ExpiresAt { get; set; }

    // User details
    public UserResponseDto User { get; set; } = null!;
}