using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterRequest request);

    Task<User> LoginAsync(LoginRequest request);
}