using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<bool> EmailExistsAsync(string email);

    Task<Role?> GetRoleByNameAsync(string roleName);

    Task<User?> GetByEmailAsync(string email);

    Task<User> AddUserAsync(User user);

    Task<User?> GetByIdAsync(int userId);
}