
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();

    Task<Student?> GetByIdAsync(int id);

    Task<Student?> GetByUserIdAsync(string userId);

    Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId);

    Task<IEnumerable<Student>> GetPendingAsync(int collegeId);

    Task AddAsync(Student student);

    Task UpdateAsync(Student student);

    Task<bool> ExistsAsync(int id);

    Task<bool> RegisterNumberExistsAsync(string registerNumber);
}
