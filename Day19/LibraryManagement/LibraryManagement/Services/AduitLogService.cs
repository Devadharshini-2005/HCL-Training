using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _auditLogRepository;

    public AuditLogService(AuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    // Add audit log
    public void AddAuditLog(AuditLog auditLog)
    {
        ValidateAuditLog(auditLog);

        auditLog.Action = auditLog.Action.Trim();
        auditLog.EntityName = auditLog.EntityName.Trim();
        auditLog.Description = auditLog.Description.Trim();

        if (auditLog.CreatedAt == default)
        {
            auditLog.CreatedAt = DateTime.Now;
        }

        _auditLogRepository.AddAuditLog(auditLog);
    }

    // Get all audit logs
    public List<AuditLog> GetAllAuditLogs()
    {
        return _auditLogRepository.GetAllAuditLogs();
    }

    // Get audit log by ID
    public AuditLog GetAuditLogById(int auditLogId)
    {
        if (auditLogId <= 0)
        {
            throw new ArgumentException("Audit Log ID must be greater than zero.");
        }

        var auditLog = _auditLogRepository.GetAuditLogById(auditLogId);

        if (auditLog == null)
        {
            throw new KeyNotFoundException(
                $"Audit log with ID {auditLogId} was not found.");
        }

        return auditLog;
    }

    // Get audit logs by entity
    public List<AuditLog> GetAuditLogsByEntity(string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName))
        {
            throw new ArgumentException("Entity name cannot be empty.");
        }

        return _auditLogRepository.GetAuditLogsByEntity(entityName.Trim());
    }

    // Get audit logs by action
    public List<AuditLog> GetAuditLogsByAction(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            throw new ArgumentException("Action cannot be empty.");
        }

        return _auditLogRepository.GetAuditLogsByAction(action.Trim());
    }

    // Validation
    private static void ValidateAuditLog(AuditLog auditLog)
    {
        ArgumentNullException.ThrowIfNull(auditLog);

        if (string.IsNullOrWhiteSpace(auditLog.Action))
        {
            throw new ArgumentException("Action cannot be empty.");
        }

        if (auditLog.Action.Trim().Length > 50)
        {
            throw new ArgumentException("Action cannot exceed 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(auditLog.EntityName))
        {
            throw new ArgumentException("Entity name cannot be empty.");
        }

        if (auditLog.EntityName.Trim().Length > 100)
        {
            throw new ArgumentException("Entity name cannot exceed 100 characters.");
        }

        if (auditLog.EntityId <= 0)
        {
            throw new ArgumentException("Entity ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(auditLog.Description))
        {
            throw new ArgumentException("Description cannot be empty.");
        }

        if (auditLog.Description.Trim().Length > 500)
        {
            throw new ArgumentException("Description cannot exceed 500 characters.");
        }
    }
}