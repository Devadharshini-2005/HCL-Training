
using FoodHub.Models;

namespace FoodHub.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
