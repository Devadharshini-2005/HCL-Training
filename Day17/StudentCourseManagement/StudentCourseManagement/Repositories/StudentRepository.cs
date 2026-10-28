using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class StudentRepository
{
    public async Task<int> AddAsync(Student student)
    {
        const string sql = """
            INSERT INTO Students (Name, Email, Age, DepartmentId)
            VALUES (@Name, @Email, @Age, @DepartmentId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@Name", student.Name);
        cmd.Parameters.AddWithValue("@Email", student.Email);
        cmd.Parameters.AddWithValue("@Age", student.Age);
        cmd.Parameters.AddWithValue("@DepartmentId", student.DepartmentId);

        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<Student>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Students ORDER BY StudentId";

        var students = new List<Student>();

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        const string sql =
            "SELECT * FROM Students WHERE StudentId = @Id";

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Id", id);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Student
        {
            StudentId = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            Age = reader.GetInt32(3),
            DepartmentId = reader.GetInt32(4),
            AdmissionDate = reader.GetDateTime(5),
            IsActive = reader.GetBoolean(6)
        };
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        const string sql = """
            UPDATE Students
            SET IsActive = 0
            WHERE StudentId = @Id AND IsActive = 1
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}