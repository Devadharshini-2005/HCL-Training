using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Models;

namespace TrainingManagementSystem.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(TrainingDbContext db)
    {
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new UserAccount { Name = "Training Admin", Email = "admin@training.local", Password = "Admin@123", Role = "Admin", ContactNumber = "9876543210" },
                new UserAccount { Name = "Demo Student", Email = "student@training.local", Password = "Student@123", Role = "Student", ContactNumber = "9876543211" }
            );
        }

        if (!await db.Courses.AnyAsync())
        {
            db.Courses.AddRange(
                new Course { CourseCode = "CS101", Title = "C# Fundamentals", Fee = 4500, DurationDays = 15, TotalSeats = 30 },
                new Course { CourseCode = "WEB201", Title = "Frontend Web Development", Fee = 6000, DurationDays = 20, TotalSeats = 25 },
                new Course { CourseCode = "DB301", Title = "SQL Server Database", Fee = 5500, DurationDays = 12, TotalSeats = 20 },
                new Course { CourseCode = "DOT401", Title = ".NET Full Stack", Fee = 9000, DurationDays = 30, TotalSeats = 20 },
                new Course { CourseCode = "JAVA101", Title = "Java Programming", Fee = 5000, DurationDays = 18, TotalSeats = 25 }
            );
        }
        await db.SaveChangesAsync();
    }
}
