using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(StudentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Student>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<Student?> GetByIdAsync(int id)
        => _repository.GetByIdAsync(id);

    public Task<int> RegisterAsync(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required.");

        if (!student.Email.Contains('@'))
            throw new ArgumentException("Enter a valid email.");

        if (student.Age is < 17 or > 60)
            throw new ArgumentException("Age must be between 17 and 60.");

        if (student.DepartmentId <= 0)
            throw new ArgumentException("Select a valid department.");

        return _repository.AddAsync(student);
    }

    public Task<bool> DeactivateAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid student ID.");

        return _repository.DeactivateAsync(id);
    }
}