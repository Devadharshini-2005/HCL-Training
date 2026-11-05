
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _context;

    public CollegeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all colleges
    public async Task<IEnumerable<College>> GetAllAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .ToListAsync();
    }

    // Get college by ID
    public async Task<College?> GetByIdAsync(int id)
    {
        return await _context.Colleges
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // Get college by college code
    public async Task<College?> GetByCollegeCodeAsync(string collegeCode)
    {
        return await _context.Colleges
            .FirstOrDefaultAsync(c => c.CollegeCode == collegeCode);
    }

    // Get pending colleges
    public async Task<IEnumerable<College>> GetPendingCollegesAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .Where(c => c.Status == Models.Enums.CollegeStatus.Pending)
            .ToListAsync();
    }

    // Add college
    public async Task AddAsync(College college)
    {
        await _context.Colleges.AddAsync(college);
        await _context.SaveChangesAsync();
    }

    // Update college
    public async Task UpdateAsync(College college)
    {
        _context.Colleges.Update(college);
        await _context.SaveChangesAsync();
    }

    // Check college existence
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Colleges
            .AnyAsync(c => c.Id == id);
    }

    // Check duplicate college code
    public async Task<bool> CollegeCodeExistsAsync(string collegeCode)
    {
        return await _context.Colleges
            .AnyAsync(c => c.CollegeCode == collegeCode);
    }
}
