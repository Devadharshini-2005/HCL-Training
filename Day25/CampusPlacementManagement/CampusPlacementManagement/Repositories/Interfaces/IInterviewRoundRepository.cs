
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IInterviewRoundRepository
{
    Task<List<InterviewRound>> GetByJobOpeningIdAsync(int jobOpeningId);

    Task<InterviewRound?> GetByIdAsync(int id);

    Task<InterviewRound?> GetByJobAndRoundNumberAsync(
        int jobOpeningId,
        int roundNumber);

    Task AddAsync(InterviewRound interviewRound);

    Task UpdateAsync(InterviewRound interviewRound);

    Task<bool> ExistsAsync(int id);
}
