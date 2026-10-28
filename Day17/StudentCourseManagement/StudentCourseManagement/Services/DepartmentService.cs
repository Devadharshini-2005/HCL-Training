using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _repository;

    public DepartmentService(DepartmentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Department>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<int> AddAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
            throw new ArgumentException("Department name is required.");

        return _repository.AddAsync(department);
    }

    public Task<bool> DeactivateAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid department ID.");

        return _repository.DeactivateAsync(id);
    }
}