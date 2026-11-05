
using CampusPlacementManagement.DTOs.RecruitmentDrives;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IRecruitmentDriveService
{
    Task<List<RecruitmentDriveResponseDto>> GetAllAsync(
        string userId,
        string userRole);

    Task<RecruitmentDriveResponseDto?> GetByIdAsync(
        int id,
        string userId,
        string userRole);

    Task<RecruitmentDriveResponseDto> CreateAsync(
        CreateRecruitmentDriveRequestDto request,
        string companyHRUserId);

    Task<RecruitmentDriveResponseDto> UpdateAsync(
        int id,
        UpdateRecruitmentDriveRequestDto request,
        string companyHRUserId);

    Task CancelAsync(
        int id,
        string companyHRUserId);
}
