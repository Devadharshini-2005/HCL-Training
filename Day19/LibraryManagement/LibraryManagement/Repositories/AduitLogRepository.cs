using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class AuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    // Add a new audit log
    public void AddAuditLog(AuditLog auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        _context.SaveChanges();
    }

    // Get all audit logs
    public List<AuditLog> GetAllAuditLogs()
    {
        return _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
    }

    // Get audit log by ID
    public AuditLog? GetAuditLogById(int auditLogId)
    {
        return _context.AuditLogs
            .AsNoTracking()
            .FirstOrDefault(a => a.AuditLogId == auditLogId);
    }

    // Get audit logs by entity name
    public List<AuditLog> GetAuditLogsByEntity(string entityName)
    {
        return _context.AuditLogs
            .AsNoTracking()
            .Where(a => a.EntityName == entityName)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
    }

    // Get audit logs by action
    public List<AuditLog> GetAuditLogsByAction(string action)
    {
        return _context.AuditLogs
            .AsNoTracking()
            .Where(a => a.Action == action)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
    }
}