
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();

    Task<Company?> GetByIdAsync(int id);

    Task<IEnumerable<Company>> GetPendingAsync();

    Task AddAsync(Company company);

    Task UpdateAsync(Company company);

    Task<bool> ExistsAsync(int id);

    Task<bool> EmailExistsAsync(string email);
}
