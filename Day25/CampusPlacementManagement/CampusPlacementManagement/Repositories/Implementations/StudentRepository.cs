
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all students
    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.College)
            .ToListAsync();
    }

    // Get student by ID
    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    // Get student using Identity User ID
    public async Task<Student?> GetByUserIdAsync(string userId)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    // Get students belonging to a college
    public async Task<IEnumerable<Student>> GetByCollegeIdAsync(
        int collegeId)
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.College)
            .Where(s => s.CollegeId == collegeId)
            .ToListAsync();
    }

    // Get pending students belonging to a college
    public async Task<IEnumerable<Student>> GetPendingAsync(
        int collegeId)
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.College)
            .Where(s =>
                s.CollegeId == collegeId &&
                s.User.AccountStatus == Models.Enums.AccountStatus.Pending)
            .ToListAsync();
    }

    // Add student
    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    // Update student
    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    // Check student existence
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Students
            .AnyAsync(s => s.Id == id);
    }

    // Check duplicate register number
    public async Task<bool> RegisterNumberExistsAsync(
        string registerNumber)
    {
        return await _context.Students
            .AnyAsync(s => s.RegisterNumber == registerNumber);
    }
}
