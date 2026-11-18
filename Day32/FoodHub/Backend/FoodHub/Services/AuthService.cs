
using FoodHub.DTOs.Auth.Requests;
using FoodHub.Models;
using FoodHub.Repositories.Interfaces;
using FoodHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using FoodHub.Constants;

namespace FoodHub.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        var existingUser =
            await _authRepository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            Role = UserRoles.Customer
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        await _authRepository.AddAsync(user);

        return true;
    }

    public async Task<string?> LoginAsync(LoginRequestDto request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        var user = await _authRepository.GetByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        string token = _jwtTokenService.GenerateToken(user);

        return token;
    }
}
