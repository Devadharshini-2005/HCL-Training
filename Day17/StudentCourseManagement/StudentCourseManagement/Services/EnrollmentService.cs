using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository _repository;

    public EnrollmentService(EnrollmentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Enrollment>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<int> EnrollAsync(int studentId, int courseId)
    {
        if (studentId <= 0 || courseId <= 0)
            throw new ArgumentException("Invalid student or course ID.");

        return _repository.AddAsync(new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            Status = "Enrolled"
        });
    }

    public Task<bool> CompleteAsync(int id)
        => _repository.UpdateStatusAsync(id, "Completed");

    public Task<bool> CancelAsync(int id)
        => _repository.UpdateStatusAsync(id, "Cancelled");
}