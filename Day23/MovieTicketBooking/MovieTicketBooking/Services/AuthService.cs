using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Exceptions;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly PasswordService _passwordService;

    public AuthService(
        IAuthRepository authRepository,
        PasswordService passwordService)
    {
        _authRepository = authRepository;
        _passwordService = passwordService;
    }

    // User Registration
    public async Task<User> RegisterAsync(RegisterRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        // Check whether email already exists
        bool emailExists = await _authRepository.EmailExistsAsync(email);

        if (emailExists)
        {
            throw new BusinessRuleException("Email is already registered.");
        }

        // Get Customer role
        Role? customerRole =
            await _authRepository.GetRoleByNameAsync("Customer");

        if (customerRole == null)
        {
            throw new BusinessRuleException(
                "Customer role is not available. Please seed the roles first.");
        }

        // Create user
        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PhoneNumber = request.PhoneNumber,
            RoleId = customerRole.RoleId
        };

        // Hash password before saving
        user.PasswordHash =
            _passwordService.HashPassword(user, request.Password);

        return await _authRepository.AddUserAsync(user);
    }

    // User Login
    public async Task<User> LoginAsync(LoginRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        // Find user by email
        User? user = await _authRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new BusinessRuleException("Invalid email or password.");
        }

        // Verify password
        bool isPasswordValid =
            _passwordService.VerifyPassword(user, request.Password);

        if (!isPasswordValid)
        {
            throw new BusinessRuleException("Invalid email or password.");
        }

        return user;
    }
}