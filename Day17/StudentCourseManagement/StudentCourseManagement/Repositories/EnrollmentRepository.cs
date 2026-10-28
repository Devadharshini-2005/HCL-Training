using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class EnrollmentRepository
{
    public async Task<int> AddAsync(Enrollment enrollment)
    {
        const string sql = """
            INSERT INTO Enrollments (StudentId, CourseId, Status)
            SELECT @StudentId, @CourseId, @Status
            WHERE EXISTS
            (
                SELECT 1 FROM Students
                WHERE StudentId = @StudentId AND IsActive = 1
            )
            AND EXISTS
            (
                SELECT 1 FROM Courses
                WHERE CourseId = @CourseId AND IsActive = 1
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@StudentId", enrollment.StudentId);
        cmd.Parameters.AddWithValue("@CourseId", enrollment.CourseId);
        cmd.Parameters.AddWithValue("@Status", enrollment.Status);

        object? result = await cmd.ExecuteScalarAsync();

        if (result == null || result == DBNull.Value)
            throw new InvalidOperationException(
                "Student or course does not exist or is inactive.");

        return Convert.ToInt32(result);
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        const string sql =
            "SELECT * FROM Enrollments ORDER BY EnrollmentId";

        var enrollments = new List<Enrollment>();

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        if (status != "Completed" && status != "Cancelled")
            throw new ArgumentException("Invalid enrollment status.");

        const string sql = """
            UPDATE Enrollments
            SET Status = @Status
            WHERE EnrollmentId = @Id AND Status = 'Enrolled'
            """;

        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();

        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@Id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}
