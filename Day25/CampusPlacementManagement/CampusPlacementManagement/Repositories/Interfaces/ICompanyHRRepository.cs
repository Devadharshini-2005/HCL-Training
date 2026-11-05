
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface ICompanyHRRepository
{
    Task<IEnumerable<CompanyHR>> GetAllAsync();

    Task<CompanyHR?> GetByIdAsync(int id);

    Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId);

    Task AddAsync(CompanyHR companyHR);

    Task UpdateAsync(CompanyHR companyHR);

    Task<bool> ExistsAsync(int id);
}
