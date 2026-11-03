using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Services;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtService _jwtService;

    public AuthController(
        IAuthService authService,
        JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    // User Registration
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request);

        return Ok(new
        {
            message = "Registration successful.",
            userId = user.UserId,
            fullName = user.FullName,
            email = user.Email,
            role = user.Role.RoleName
        });
    }

    // User Login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _authService.LoginAsync(request);

        var (token, expiresAt) = _jwtService.GenerateToken(user);

        var response = new AuthResponse
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.RoleName,
            Token = token,
            ExpiresAt = expiresAt
        };

        return Ok(response);
    }
}