using Microsoft.EntityFrameworkCore;
using WebApiPractice.Data;
using WebApiPractice.Models;

namespace WebApiPractice.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(s => s.Department)
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == id);
    }



    public async Task<bool> EmailExistsAsync(
        string email,
        int? excludeStudentId = null)
    {
        return await _context.Students.AnyAsync(s =>
            s.Email == email &&
            s.Id != excludeStudentId);
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student)
    {
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Student>> SearchByNameAsync(string search)
    {
        return await _context.Students
            .Include(s => s.Department)
            .Where(s => s.Name.Contains(search))
            .ToListAsync();
    }

    public async Task<List<Student>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Students
            .Include(s => s.Department)
            .Where(s => s.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<List<Student>> GetPagedAsync(
    int pageNumber,
    int pageSize)
    {
        return await _context.Students
            .Include(s => s.Department)
            .OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Students.CountAsync();
    }

    public async Task<List<Student>> GetFilteredAndPagedAsync(
    string? search,
    int? departmentId,
    string sortBy,
    string sortOrder,
    int pageNumber,
    int pageSize)
    {
        IQueryable<Student> query = _context.Students
            .Include(s => s.Department);

        // 1. Search by student name
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.Name.Contains(search));
        }

        // 2. Filter by department
        if (departmentId.HasValue)
        {
            query = query.Where(s => s.DepartmentId == departmentId.Value);
        }

        // 3. Sorting
        bool descending = sortOrder == "desc";

        query = sortBy.ToLower() switch
        {
            "name" => descending
                ? query.OrderByDescending(s => s.Name)
                : query.OrderBy(s => s.Name),

            "age" => descending
                ? query.OrderByDescending(s => s.Age)
                : query.OrderBy(s => s.Age),

            _ => descending
                ? query.OrderByDescending(s => s.Id)
                : query.OrderBy(s => s.Id)
        };

        // 4. Pagination
        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetFilteredCountAsync(
    string? search,
    int? departmentId)
    {
        IQueryable<Student> query = _context.Students;

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.Name.Contains(search));
        }

        if (departmentId.HasValue)
        {
            query = query.Where(s => s.DepartmentId == departmentId.Value);
        }

        return await query.CountAsync();
    }

}

