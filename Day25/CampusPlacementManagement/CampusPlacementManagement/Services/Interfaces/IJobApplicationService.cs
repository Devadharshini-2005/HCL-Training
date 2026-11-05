using CampusPlacementManagement.DTOs.Applications;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IJobApplicationService
{
    Task<ApplyJobResponseDto> ApplyAsync(
        int jobOpeningId,
        string studentUserId);

    Task<IEnumerable<ApplyJobResponseDto>> GetMyApplicationsAsync(
        string studentUserId);

    Task<IEnumerable<ApplyJobResponseDto>> GetApplicationsByJobAsync(
        int jobOpeningId,
        string companyHRUserId);

    Task<IEnumerable<ApplyJobResponseDto>> GetApplicationsByCompanyAsync(
        string companyHRUserId);

    Task<ApplyJobResponseDto?> UpdateStatusAsync(
        int applicationId,
        UpdateJobApplicationStatusRequestDto request,
        string companyHRUserId);

    Task<bool> WithdrawAsync(
        int applicationId,
        string studentUserId);

    Task<IEnumerable<ApplicationStatusHistoryResponseDto>>
        GetApplicationHistoryAsync(
            int applicationId,
            string userId,
            string userRole);

    Task<int?> GetCompanyIdByJobIdAsync(int jobOpeningId);

    Task<int?> GetCompanyIdByApplicationIdAsync(int applicationId);

    Task<ApplicationAuthorizationDto?> GetApplicationForAuthorizationAsync(
    int applicationId);
}