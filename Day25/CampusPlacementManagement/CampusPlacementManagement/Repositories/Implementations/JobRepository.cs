
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobOpening>> GetAllAsync()
    {
        return await _context.JobOpenings
            .AsNoTracking()
            .Include(j => j.Company)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .Include(j => j.EligibleDepartments)
            .Include(j => j.EligibleGraduationYears)
            .ToListAsync();
    }

    public async Task<JobOpening?> GetByIdAsync(int id)
    {
        return await _context.JobOpenings
            .Include(j => j.Company)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .Include(j => j.EligibleDepartments)
            .Include(j => j.EligibleGraduationYears)
            .FirstOrDefaultAsync(j => j.Id == id);
    }


    public async Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId)
    {
        return await _context.JobOpenings
            .AsNoTracking()
            .Where(j => j.CompanyId == companyId)
            .Include(j => j.Company)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .Include(j => j.EligibleDepartments)
            .Include(j => j.EligibleGraduationYears)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobOpening>> GetPublishedAsync()
    {
        return await _context.JobOpenings
            .AsNoTracking()
            .Where(j => j.Status == Models.Enums.JobStatus.Published)
            .Include(j => j.Company)
            .Include(j => j.TargetColleges)
            .Include(j => j.EligibleDepartments)
            .Include(j => j.EligibleGraduationYears)
            .ToListAsync();
    }

    public async Task AddAsync(JobOpening job)
    {
        await _context.JobOpenings.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobOpening job)
    {
        _context.JobOpenings.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.JobOpenings
            .AnyAsync(j => j.Id == id);
    }
}
