using CampusPlacementManagement.DTOs.Auth;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IAuthService
{
    // User login
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    // Generate a new access token using refresh token
    Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);

    // Revoke refresh token during logout
    Task LogoutAsync(string refreshToken);
}