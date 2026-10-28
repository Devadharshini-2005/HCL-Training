using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class AuditLogRepository
{
    public async Task AddAsync(AuditLog log)
    {
        const string sql = """
            INSERT INTO AuditLogs
                (EntityName, EntityId, Action, Details)
            VALUES (@Entity, @Id, @Action, @Details)
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@Entity", log.EntityName);
        cmd.Parameters.AddWithValue("@Id", log.EntityId);
        cmd.Parameters.AddWithValue("@Action", log.Action);
        cmd.Parameters.AddWithValue("@Details",
            (object?)log.Details ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<AuditLog>> GetRecentAsync()
    {
        const string sql = """
            SELECT TOP 50 *
            FROM AuditLogs
            ORDER BY CreatedAt DESC
            """;

        var logs = new List<AuditLog>();

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            logs.Add(new AuditLog
            {
                AuditId = reader.GetInt64(0),
                EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2),
                Action = reader.GetString(3),
                Details = reader.IsDBNull(4)
                    ? null : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            });
        }

        return logs;
    }
}