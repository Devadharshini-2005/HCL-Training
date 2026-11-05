
using CampusPlacementManagement.DTOs.Interviews;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IInterviewRoundService
{
    Task<List<InterviewRoundResponseDto>> GetByJobOpeningIdAsync(
        int jobOpeningId,
        string userId,
        string userRole);

    Task<InterviewRoundResponseDto> CreateAsync(
        int jobOpeningId,
        CreateInterviewRoundRequestDto request,
        string companyHRUserId);

    Task<InterviewRoundResponseDto> UpdateAsync(
        int id,
        UpdateInterviewRoundRequestDto request,
        string companyHRUserId);
}
