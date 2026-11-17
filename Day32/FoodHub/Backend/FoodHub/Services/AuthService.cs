
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
        // Normalize email to avoid duplicate registrations
        string email = request.Email.Trim().ToLowerInvariant();

        // Check whether the email already exists
        var existingUser =
            await _authRepository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            return false;
        }

        // Create the new user
        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            Role = "Customer"
        };

        // Hash the password before saving
        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        // Save user in the database
        await _authRepository.AddAsync(user);

        return true;
    }
}
