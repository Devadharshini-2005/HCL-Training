using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Data;

namespace TrainingManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(TrainingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var courses = await db.Courses.ToListAsync();
        var enrollments = await db.Enrollments.ToListAsync();
        var payments = await db.Payments.ToListAsync();

        var occupancy = courses.Select(c => new {
            c.CourseCode, c.Title, c.TotalSeats, OccupiedSeats = c.EnrolledCount,
            AvailableSeats = c.AvailableSeats,
            OccupancyPercentage = c.TotalSeats == 0 ? 0 : Math.Round((decimal)c.EnrolledCount / c.TotalSeats * 100, 2)
        }).ToList();
        decimal expectedFees = 0;
        foreach (var enrollment in enrollments)
            if (enrollment.Status == "Active") expectedFees += enrollment.FinalFee;

        return Ok(new {
            Occupancy = occupancy,
            ExpectedFees = expectedFees,
            SuccessfulPaymentAttempts = payments.Count(p => p.Status == "Successful"),
            FailedPaymentAttempts = payments.Count(p => p.Status == "Failed"),
            TotalCollected = payments.Where(p => p.Status == "Successful").Sum(p => p.Amount),
            ActiveEnrollments = enrollments.Count(e => e.Status == "Active")
        });
    }
}
