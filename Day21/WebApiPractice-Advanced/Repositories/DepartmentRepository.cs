using Microsoft.EntityFrameworkCore;
using WebApiPractice.Data;
using WebApiPractice.Models;

namespace WebApiPractice.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.Students)
            .OrderBy(d => d.DepartmentId)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.DepartmentId == id);
    }

    public async Task<bool> ExistsByNameAsync(string departmentName)
    {
        return await _context.Departments
            .AnyAsync(d => d.DepartmentName == departmentName);
    }

    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department department)
    {
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Department>> GetFilteredAndPagedAsync(
    string? search,
    int pageNumber,
    int pageSize)
    {
        var query = _context.Departments
            .Include(d => d.Students)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(d =>
                d.DepartmentName.Contains(search));
        }

        return await query
            .OrderBy(d => d.DepartmentId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetFilteredCountAsync(string? search)
    {
        var query = _context.Departments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(d =>
                d.DepartmentName.Contains(search));
        }

        return await query.CountAsync();
    }

}