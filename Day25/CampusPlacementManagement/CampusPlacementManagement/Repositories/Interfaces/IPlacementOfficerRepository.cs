
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IPlacementOfficerRepository
{
    Task<IEnumerable<PlacementOfficer>> GetAllAsync();

    Task<PlacementOfficer?> GetByIdAsync(int id);

    Task<PlacementOfficer?> GetByUserIdAsync(string userId);

    Task<IEnumerable<PlacementOfficer>> GetByCollegeIdAsync(int collegeId);

    Task AddAsync(PlacementOfficer placementOfficer);

    Task UpdateAsync(PlacementOfficer placementOfficer);

    Task<bool> ExistsAsync(int id);
}
