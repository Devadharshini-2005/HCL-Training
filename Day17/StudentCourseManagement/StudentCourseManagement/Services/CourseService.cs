using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class CourseService
{
    private readonly CourseRepository _repository;

    public CourseService(CourseRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Course>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<int> AddAsync(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.CourseName))
            throw new ArgumentException("Course name is required.");

        if (course.DurationMonths <= 0)
            throw new ArgumentException("Duration must be positive.");

        if (course.Fee < 0)
            throw new ArgumentException("Fee cannot be negative.");

        if (course.DepartmentId <= 0)
            throw new ArgumentException("Select a valid department.");

        return _repository.AddAsync(course);
    }

    public Task<bool> DeactivateAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid course ID.");

        return _repository.DeactivateAsync(id);
    }
}