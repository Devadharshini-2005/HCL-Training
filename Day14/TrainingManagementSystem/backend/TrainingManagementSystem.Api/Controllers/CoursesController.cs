using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Data;
using TrainingManagementSystem.Api.Models;

namespace TrainingManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(TrainingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sort)
    {
        var courses = await db.Courses.OrderBy(c => c.Id).ToListAsync();
        if (!string.IsNullOrWhiteSpace(search))
            courses = courses.Where(c => c.CourseCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                         c.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        if (string.Equals(sort, "asc", StringComparison.OrdinalIgnoreCase))
            courses = courses.OrderBy(c => c.Fee).ToList();
        else if (string.Equals(sort, "desc", StringComparison.OrdinalIgnoreCase))
            courses = courses.OrderByDescending(c => c.Fee).ToList();
        return Ok(courses.Select(c => new {
            c.Id, c.CourseCode, c.Title, c.Fee, c.DurationDays, c.TotalSeats,
            c.EnrolledCount, AvailableSeats = c.AvailableSeats
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CourseCode) || string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Course code and title are required." });
        if (request.Fee < 0 || request.DurationDays <= 0 || request.TotalSeats <= 0)
            return BadRequest(new { message = "Fee must be non-negative; duration and seats must be positive." });
        if (await db.Courses.AnyAsync(c => c.CourseCode.ToLower() == request.CourseCode.ToLower()))
            return Conflict(new { message = "Course code already exists." });

        var course = new Course { CourseCode = request.CourseCode.Trim().ToUpper(), Title = request.Title.Trim(),
            Fee = request.Fee, DurationDays = request.DurationDays, TotalSeats = request.TotalSeats };
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = course.Id }, course);
    }
}
