using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Data;
using TrainingManagementSystem.Api.Models;

namespace TrainingManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(TrainingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Payments.OrderByDescending(p => p.PaymentDate).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Process(PaymentRequest request)
    {
        var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.Id == request.EnrollmentId);
        if (enrollment is null) return NotFound(new { message = "Enrollment not found." });
        if (enrollment.Status == "Cancelled") return BadRequest(new { message = "Cannot pay for a cancelled enrollment." });

        // Simulated outcome: a method named "Fail" intentionally records a failed attempt.
        bool success = !string.Equals(request.Method, "Fail", StringComparison.OrdinalIgnoreCase);
        var payment = new Payment {
            EnrollmentId = enrollment.Id, Amount = enrollment.FinalFee, PaymentDate = DateTime.Now,
            Method = request.Method, Status = success ? "Successful" : "Failed"
        };
        db.Payments.Add(payment);
        if (success) enrollment.PaymentStatus = "Paid";
        await db.SaveChangesAsync();
        return Ok(new { message = success ? "Simulated payment successful." : "Simulated payment failed; attempt recorded.", payment, enrollment });
    }
}
