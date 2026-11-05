
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IJobRepository
{
    Task<IEnumerable<JobOpening>> GetAllAsync();

    Task<JobOpening?> GetByIdAsync(int id);

    Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId);

    Task<IEnumerable<JobOpening>> GetPublishedAsync();

    Task AddAsync(JobOpening job);

    Task UpdateAsync(JobOpening job);

    Task<bool> ExistsAsync(int id);
}
