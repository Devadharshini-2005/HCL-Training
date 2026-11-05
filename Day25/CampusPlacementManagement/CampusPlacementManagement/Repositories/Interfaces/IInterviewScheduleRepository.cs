
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IInterviewScheduleRepository
{
    Task<List<InterviewSchedule>> GetByApplicationIdAsync(
        int applicationId);

    Task<InterviewSchedule?> GetByIdAsync(int id);

    Task AddAsync(InterviewSchedule interviewSchedule);

    Task UpdateAsync(InterviewSchedule interviewSchedule);

    Task<bool> ExistsAsync(int id);
}
