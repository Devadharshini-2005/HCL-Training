
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class InterviewRoundRepository : IInterviewRoundRepository
{
    private readonly ApplicationDbContext _context;

    public InterviewRoundRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InterviewRound>> GetByJobOpeningIdAsync(
        int jobOpeningId)
    {
        return await _context.InterviewRounds
            .Where(r => r.JobOpeningId == jobOpeningId)
            .Include(r => r.JobOpening)
            .OrderBy(r => r.RoundNumber)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<InterviewRound?> GetByIdAsync(int id)
    {
        return await _context.InterviewRounds
            .Include(r => r.JobOpening)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<InterviewRound?> GetByJobAndRoundNumberAsync(
        int jobOpeningId,
        int roundNumber)
    {
        return await _context.InterviewRounds
            .FirstOrDefaultAsync(r =>
                r.JobOpeningId == jobOpeningId &&
                r.RoundNumber == roundNumber);
    }

    public async Task AddAsync(InterviewRound interviewRound)
    {
        await _context.InterviewRounds.AddAsync(interviewRound);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(InterviewRound interviewRound)
    {
        _context.InterviewRounds.Update(interviewRound);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.InterviewRounds
            .AnyAsync(r => r.Id == id);
    }
}
