
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IRecruitmentDriveRepository
{
    Task<List<RecruitmentDrive>> GetAllAsync();

    Task<RecruitmentDrive?> GetByIdAsync(int id);

    Task<List<RecruitmentDrive>> GetByJobOpeningIdAsync(int jobOpeningId);

    Task<List<RecruitmentDrive>> GetByCollegeIdAsync(int collegeId);

    Task AddAsync(RecruitmentDrive recruitmentDrive);

    Task UpdateAsync(RecruitmentDrive recruitmentDrive);

    Task<bool> ExistsAsync(int id);
}
