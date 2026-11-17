
using FoodHub.Models;

namespace FoodHub.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User> AddAsync(User user);
}
