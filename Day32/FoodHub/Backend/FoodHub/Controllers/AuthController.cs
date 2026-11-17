
using FoodHub.DTOs.Auth.Requests;
using FoodHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequestDto request)
    {
        var isRegistered =
            await _authService.RegisterAsync(request);

        if (!isRegistered)
        {
            return Conflict(new
            {
                message = "Email is already registered."
            });
        }

        return StatusCode(201, new
        {
            message = "User registered successfully."
        });
    }
}
