using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IRefreshTokenService
{
    Task<string> CreateRefreshTokenAsync(ApplicationUser user);

    Task<ApplicationUser?> ValidateRefreshTokenAsync(string refreshToken);

    Task<(ApplicationUser? User, string? NewRefreshToken)>
        RotateRefreshTokenAsync(string refreshToken);

    Task RevokeRefreshTokenAsync(string refreshToken);
}