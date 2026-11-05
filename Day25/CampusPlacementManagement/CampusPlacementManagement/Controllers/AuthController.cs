
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password, or account is not active."
            });
        }

        return Ok(response);
    }

    // POST: api/auth/refresh
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(
        RefreshTokenRequestDto request)
    {
        var response = await _authService.RefreshTokenAsync(
            request.RefreshToken);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid, expired, or revoked refresh token."
            });
        }

        return Ok(response);
    }

    // POST: api/auth/logout
    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        RefreshTokenRequestDto request)
    {
        await _authService.LogoutAsync(request.RefreshToken);

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }
}
