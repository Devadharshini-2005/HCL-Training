using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Data;

namespace TrainingManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearnersController(TrainingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Users.Where(u => u.Role == "Student")
            .Select(u => new { u.Id, u.Name, u.Email, u.ContactNumber }).ToListAsync());
}
