
using FoodHub.DTOs.Auth.Requests;

namespace FoodHub.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto request);
}
