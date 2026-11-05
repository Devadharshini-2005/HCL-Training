
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class InterviewScheduleRepository : IInterviewScheduleRepository
{
    private readonly ApplicationDbContext _context;

    public InterviewScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InterviewSchedule>> GetByApplicationIdAsync(
        int applicationId)
    {
        return await _context.InterviewSchedules
            .Include(i => i.JobApplication)
                .ThenInclude(a => a.Student)
                    .ThenInclude(s => s.User)
            .Include(i => i.JobApplication)
                .ThenInclude(a => a.JobOpening)
                    .ThenInclude(j => j.Company)
            .Include(i => i.InterviewRound)
                .ThenInclude(r => r.JobOpening)
            .Where(i => i.JobApplicationId == applicationId)
            .OrderBy(i => i.ScheduledAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<InterviewSchedule?> GetByIdAsync(int id)
    {
        return await _context.InterviewSchedules
            .Include(i => i.JobApplication)
                .ThenInclude(a => a.Student)
                    .ThenInclude(s => s.User)
            .Include(i => i.JobApplication)
                .ThenInclude(a => a.JobOpening)
                    .ThenInclude(j => j.Company)
            .Include(i => i.InterviewRound)
                .ThenInclude(r => r.JobOpening)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task AddAsync(InterviewSchedule interviewSchedule)
    {
        await _context.InterviewSchedules.AddAsync(interviewSchedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(InterviewSchedule interviewSchedule)
    {
        _context.InterviewSchedules.Update(interviewSchedule);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.InterviewSchedules
            .AnyAsync(i => i.Id == id);
    }
}
