using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class CourseRepository
{
    public async Task<int> AddAsync(Course course)
    {
        const string sql = """
            INSERT INTO Courses
                (CourseName, DurationMonths, Fee, DepartmentId)
            VALUES
                (@Name, @Duration, @Fee, @DepartmentId);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@Name", course.CourseName);
        cmd.Parameters.AddWithValue("@Duration", course.DurationMonths);
        cmd.Parameters.AddWithValue("@Fee", course.Fee);
        cmd.Parameters.AddWithValue("@DepartmentId", course.DepartmentId);

        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<Course>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Courses ORDER BY CourseId";

        var courses = new List<Course>();

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        const string sql = """
            UPDATE Courses
            SET IsActive = 0
            WHERE CourseId = @Id AND IsActive = 1
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}