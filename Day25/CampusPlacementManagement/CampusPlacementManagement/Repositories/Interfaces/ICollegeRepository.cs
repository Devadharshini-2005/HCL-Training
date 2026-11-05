
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface ICollegeRepository
{
    Task<IEnumerable<College>> GetAllAsync();

    Task<College?> GetByIdAsync(int id);

    Task<College?> GetByCollegeCodeAsync(string collegeCode);

    Task<IEnumerable<College>> GetPendingCollegesAsync();

    Task AddAsync(College college);

    Task UpdateAsync(College college);

    Task<bool> ExistsAsync(int id);

    Task<bool> CollegeCodeExistsAsync(string collegeCode);
}
