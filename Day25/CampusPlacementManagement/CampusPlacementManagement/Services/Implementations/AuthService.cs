
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
        _configuration = configuration;
    }

    // LOGIN
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(
            user, request.Password);

        if (!isPasswordValid)
            return null;

        if (user.AccountStatus != AccountStatus.Active)
            return null;

        return await CreateLoginResponseAsync(user);
    }

    // REFRESH TOKEN
    public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var rotationResult =
            await _refreshTokenService.RotateRefreshTokenAsync(refreshToken);

        var user = rotationResult.User;
        var newRefreshToken = rotationResult.NewRefreshToken;

        if (user == null || newRefreshToken == null)
            return null;

        if (user.AccountStatus != AccountStatus.Active)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(newRefreshToken);
            return null;
        }

        var accessToken = await _jwtTokenService.GenerateTokenAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        var expiryMinutes =
            Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]);

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            User = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.UserName ?? string.Empty,
                Roles = roles
            }
        };
    }

    // LOGOUT
    public async Task LogoutAsync(string refreshToken)
    {
        await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
    }

    // COMMON RESPONSE CREATION
    private async Task<LoginResponseDto> CreateLoginResponseAsync(
        ApplicationUser user)
    {
        var accessToken =
            await _jwtTokenService.GenerateTokenAsync(user);

        var refreshToken =
            await _refreshTokenService.CreateRefreshTokenAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        var expiryMinutes =
            Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]);

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            User = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.UserName ?? string.Empty,
                Roles = roles
            }
        };
    }
}
