
using CampusPlacementManagement.DTOs.Jobs;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IJobService
{
    Task<IEnumerable<JobResponseDto>> GetAllAsync();

    Task<JobResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(int companyId);

    Task<IEnumerable<JobResponseDto>> GetPublishedAsync(
    string studentUserId);
    Task<JobResponseDto> CreateAsync(
        CreateJobRequestDto request,
        string companyHRUserId);

    Task<JobResponseDto?> UpdateAsync(
        int id,
        UpdateJobRequestDto request,
        string companyHRUserId);

    Task<bool> PublishAsync(
        int id,
        string companyHRUserId);

    Task<bool> CloseAsync(
        int id,
        string companyHRUserId);


    Task<IEnumerable<JobResponseDto>> GetByCompanyUserIdAsync(
        string companyHRUserId);

}
