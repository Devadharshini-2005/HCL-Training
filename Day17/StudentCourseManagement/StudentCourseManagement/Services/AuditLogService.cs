using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repository;

    public AuditLogService(AuditLogRepository repository)
    {
        _repository = repository;
    }

    public Task<List<AuditLog>> GetRecentAsync()
        => _repository.GetRecentAsync();

    public Task RecordAsync(
        string entity,
        int id,
        string action,
        string? details)
    {
        return _repository.AddAsync(new AuditLog
        {
            EntityName = entity,
            EntityId = id,
            Action = action,
            Details = details,
            CreatedAt = DateTime.Now
        });
    }
}