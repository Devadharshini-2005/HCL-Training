using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Data;
using TrainingManagementSystem.Api.Models;

namespace TrainingManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(TrainingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? learnerId)
    {
        var query = db.Enrollments.AsQueryable();
        if (learnerId.HasValue) query = query.Where(e => e.LearnerId == learnerId.Value);
        return Ok(await query.OrderByDescending(e => e.EnrollmentDate).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEnrollmentRequest request)
    {
        var learner = await db.Users.FirstOrDefaultAsync(u => u.Id == request.LearnerId && u.Role == "Student");
        if (learner is null) return BadRequest(new { message = "Student account not found." });
        var course = await db.Courses.FirstOrDefaultAsync(c => c.CourseCode.ToLower() == request.CourseCode.ToLower());
        if (course is null) return NotFound(new { message = "Course not found." });
        if (course.AvailableSeats <= 0) return Conflict(new { message = "This course is full." });

        var duplicate = await db.Enrollments.AnyAsync(e => e.LearnerId == learner.Id &&
            e.CourseCode.ToLower() == course.CourseCode.ToLower() && e.Status == "Active");
        if (duplicate) return Conflict(new { message = "You are already actively enrolled in this course." });

        decimal discount = 0;
        if (request.EarlyBird) discount += course.Fee * 0.10m;
        if (request.GroupSize >= 3) discount += course.Fee * 0.05m;
        decimal surcharge = string.Equals(request.SessionType, "Classroom", StringComparison.OrdinalIgnoreCase) ? 800m : 200m;
        var finalFee = Math.Max(0, course.Fee - discount) + surcharge;

        var enrollment = new Enrollment {
            LearnerId = learner.Id, LearnerName = learner.Name, CourseCode = course.CourseCode,
            CourseTitle = course.Title, EnrollmentDate = DateTime.Now, BaseFee = course.Fee,
            DiscountAmount = discount, FinalFee = finalFee, Status = "Active", PaymentStatus = "Pending"
        };
        course.EnrolledCount++;
        db.Enrollments.Add(enrollment);
        await db.SaveChangesAsync();
        return Ok(enrollment);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.Id == id);
        if (enrollment is null) return NotFound(new { message = "Enrollment not found." });
        if (enrollment.Status == "Cancelled") return BadRequest(new { message = "Enrollment is already cancelled." });
        if (enrollment.Status == "Completed") return BadRequest(new { message = "Completed enrollment cannot be cancelled." });

        enrollment.Status = "Cancelled";
        var course = await db.Courses.FirstOrDefaultAsync(c => c.CourseCode == enrollment.CourseCode);
        if (course is not null && course.EnrolledCount > 0) course.EnrolledCount--;
        await db.SaveChangesAsync();
        return Ok(new { message = "Enrollment cancelled and one seat released.", enrollment });
    }
}
