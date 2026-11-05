
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class RecruitmentDriveRepository : IRecruitmentDriveRepository
{
    private readonly ApplicationDbContext _context;

    public RecruitmentDriveRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecruitmentDrive>> GetAllAsync()
    {
        return await _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RecruitmentDrive?> GetByIdAsync(int id)
    {
        return await _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<RecruitmentDrive>> GetByJobOpeningIdAsync(int jobOpeningId)
    {
        return await _context.RecruitmentDrives
            .Where(d => d.JobOpeningId == jobOpeningId)
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<RecruitmentDrive>> GetByCollegeIdAsync(int collegeId)
    {
        return await _context.RecruitmentDrives
            .Where(d => d.CollegeId == collegeId)
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(RecruitmentDrive recruitmentDrive)
    {
        await _context.RecruitmentDrives.AddAsync(recruitmentDrive);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RecruitmentDrive recruitmentDrive)
    {
        _context.RecruitmentDrives.Update(recruitmentDrive);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.RecruitmentDrives
            .AnyAsync(d => d.Id == id);
    }
}
