
using CampusPlacementManagement.DTOs.Interviews;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IInterviewScheduleService
{
    Task<List<InterviewScheduleResponseDto>> GetByApplicationIdAsync(
        int applicationId,
        string userId,
        string userRole);

    Task<InterviewScheduleResponseDto> CreateAsync(
        int applicationId,
        CreateInterviewScheduleRequestDto request,
        string companyHRUserId);

    Task<InterviewScheduleResponseDto> UpdateAsync(
        int id,
        UpdateInterviewScheduleRequestDto request,
        string companyHRUserId);

    Task<InterviewScheduleResponseDto> UpdateFeedbackAsync(
        int id,
        UpdateInterviewFeedbackRequestDto request,
        string companyHRUserId);
}
