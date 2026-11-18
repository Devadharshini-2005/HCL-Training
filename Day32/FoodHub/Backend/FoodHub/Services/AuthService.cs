
using FoodHub.DTOs.Auth.Requests;
using FoodHub.Models;
using FoodHub.Repositories.Interfaces;
using FoodHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FoodHub.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
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
            Role = "Customer"
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        await _authRepository.AddAsync(user);

        return true;
    }

    public async Task<bool> LoginAsync(LoginRequestDto request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        var user = await _authRepository.GetByEmailAsync(email);

        if (user == null)
        {
            return false;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        return result != PasswordVerificationResult.Failed;
    }
}
