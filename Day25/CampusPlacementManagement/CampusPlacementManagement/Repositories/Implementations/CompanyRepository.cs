
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all companies
    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies
            .AsNoTracking()
            .ToListAsync();
    }

    // Get company by ID
    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // Get companies awaiting approval
    public async Task<IEnumerable<Company>> GetPendingAsync()
    {
        return await _context.Companies
            .AsNoTracking()
            .Where(c => c.Status == CompanyStatus.Pending)
            .ToListAsync();
    }

    // Add a new company
    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    // Update company details
    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    // Check whether company exists
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Companies
            .AnyAsync(c => c.Id == id);
    }

    // Check whether company email already exists
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Companies
            .AnyAsync(c => c.Email == email);
    }
}
