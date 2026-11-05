using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IJobApplicationRepository
{
    Task<IEnumerable<JobApplication>> GetAllAsync();

    Task<JobApplication?> GetByIdAsync(int id);

    Task<JobApplication?> GetByStudentAndJobAsync(
        int studentId,
        int jobOpeningId);

    Task<IEnumerable<JobApplication>> GetByStudentIdAsync(
        int studentId);

    Task<IEnumerable<JobApplication>> GetByJobIdAsync(
        int jobOpeningId);

    Task AddAsync(JobApplication application);

    Task UpdateAsync(JobApplication application);

    Task<bool> ExistsAsync(int id);
}