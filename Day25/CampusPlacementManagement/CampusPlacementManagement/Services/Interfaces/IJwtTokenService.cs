
using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
