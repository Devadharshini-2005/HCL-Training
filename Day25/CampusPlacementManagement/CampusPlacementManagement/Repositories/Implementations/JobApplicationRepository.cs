using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public JobApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        return await _context.JobApplications
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Student)
                .ThenInclude(s => s.College)
            .Include(a => a.JobOpening)
                .ThenInclude(j => j.Company)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<JobApplication?> GetByIdAsync(int id)
    {
        return await _context.JobApplications
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Student)
                .ThenInclude(s => s.College)
            .Include(a => a.JobOpening)
                .ThenInclude(j => j.Company)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<JobApplication?> GetByStudentAndJobAsync(
        int studentId,
        int jobOpeningId)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(a =>
                a.StudentId == studentId &&
                a.JobOpeningId == jobOpeningId);
    }

    public async Task<IEnumerable<JobApplication>> GetByStudentIdAsync(
        int studentId)
    {
        return await _context.JobApplications
            .Where(a => a.StudentId == studentId)
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.JobOpening)
                .ThenInclude(j => j.Company)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByJobIdAsync(
        int jobOpeningId)
    {
        return await _context.JobApplications
            .Where(a => a.JobOpeningId == jobOpeningId)
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Student)
                .ThenInclude(s => s.College)
            .Include(a => a.JobOpening)
                .ThenInclude(j => j.Company)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(JobApplication application)
    {
        await _context.JobApplications.AddAsync(application);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobApplication application)
    {
        _context.JobApplications.Update(application);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.JobApplications
            .AnyAsync(a => a.Id == id);
    }
}