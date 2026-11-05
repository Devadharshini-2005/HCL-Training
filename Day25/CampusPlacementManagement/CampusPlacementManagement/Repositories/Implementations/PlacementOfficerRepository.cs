
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class PlacementOfficerRepository : IPlacementOfficerRepository
{
    private readonly ApplicationDbContext _context;

    public PlacementOfficerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlacementOfficer>> GetAllAsync()
    {
        return await _context.PlacementOfficers
            .AsNoTracking()
            .Include(po => po.User)
            .Include(po => po.College)
            .ToListAsync();
    }

    public async Task<PlacementOfficer?> GetByIdAsync(int id)
    {
        return await _context.PlacementOfficers
            .Include(po => po.User)
            .Include(po => po.College)
            .FirstOrDefaultAsync(po => po.Id == id);
    }

    public async Task<PlacementOfficer?> GetByUserIdAsync(string userId)
    {
        return await _context.PlacementOfficers
            .Include(po => po.User)
            .Include(po => po.College)
            .FirstOrDefaultAsync(po => po.UserId == userId);
    }

    public async Task<IEnumerable<PlacementOfficer>> GetByCollegeIdAsync(int collegeId)
    {
        return await _context.PlacementOfficers
            .AsNoTracking()
            .Include(po => po.User)
            .Include(po => po.College)
            .Where(po => po.CollegeId == collegeId)
            .ToListAsync();
    }

    public async Task AddAsync(PlacementOfficer placementOfficer)
    {
        await _context.PlacementOfficers.AddAsync(placementOfficer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PlacementOfficer placementOfficer)
    {
        _context.PlacementOfficers.Update(placementOfficer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PlacementOfficers
            .AnyAsync(po => po.Id == id);
    }
}
