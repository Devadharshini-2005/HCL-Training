using WebApiPractice.Models;

namespace WebApiPractice.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();

    Task<Department?> GetByIdAsync(int id);

    Task<bool> ExistsByNameAsync(string departmentName);

    Task AddAsync(Department department);

    Task UpdateAsync(Department department);

    Task DeleteAsync(Department department);

    Task<List<Department>> GetFilteredAndPagedAsync(
    string? search,
    int pageNumber,
    int pageSize);

    Task<int> GetFilteredCountAsync(string? search);
}