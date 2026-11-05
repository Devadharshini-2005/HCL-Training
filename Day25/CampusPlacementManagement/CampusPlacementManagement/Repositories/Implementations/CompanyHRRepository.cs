
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class CompanyHRRepository : ICompanyHRRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyHRRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all Company HR records
    public async Task<IEnumerable<CompanyHR>> GetAllAsync()
    {
        return await _context.CompanyHRs
            .AsNoTracking()
            .Include(hr => hr.User)
            .Include(hr => hr.Company)
            .ToListAsync();
    }

    // Get Company HR by ID
    public async Task<CompanyHR?> GetByIdAsync(int id)
    {
        return await _context.CompanyHRs
            .Include(hr => hr.User)
            .Include(hr => hr.Company)
            .FirstOrDefaultAsync(hr => hr.Id == id);
    }

    // Get HR users belonging to a company
    public async Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(
        int companyId)
    {
        return await _context.CompanyHRs
            .AsNoTracking()
            .Include(hr => hr.User)
            .Include(hr => hr.Company)
            .Where(hr => hr.CompanyId == companyId)
            .ToListAsync();
    }

    // Add Company HR record
    public async Task AddAsync(CompanyHR companyHR)
    {
        await _context.CompanyHRs.AddAsync(companyHR);
        await _context.SaveChangesAsync();
    }

    // Update Company HR record
    public async Task UpdateAsync(CompanyHR companyHR)
    {
        _context.CompanyHRs.Update(companyHR);
        await _context.SaveChangesAsync();
    }

    // Check whether HR record exists
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.CompanyHRs
            .AnyAsync(hr => hr.Id == id);
    }
}
