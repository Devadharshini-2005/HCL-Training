
using FoodHub.Data;
using FoodHub.Models;
using FoodHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly FoodHubDbContext _context;

    public AuthRepository(FoodHubDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }
}
