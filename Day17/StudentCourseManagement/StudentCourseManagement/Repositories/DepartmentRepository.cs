using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class DepartmentRepository
{
    public async Task<int> AddAsync(Department department)
    {
        const string sql = """
            INSERT INTO Departments (DepartmentName, Location)
            VALUES (@Name, @Location);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@Name", department.DepartmentName);
        cmd.Parameters.AddWithValue("@Location",
            (object?)department.Location ?? DBNull.Value);

        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<Department>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Departments ORDER BY DepartmentId";

        var departments = new List<Department>();

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return departments;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        const string sql = """
            UPDATE Departments
            SET IsActive = 0
            WHERE DepartmentId = @Id AND IsActive = 1
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}