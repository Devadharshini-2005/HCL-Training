using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Api.Data;
using TrainingManagementSystem.Api.Models;

namespace TrainingManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(TrainingDbContext db) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(u =>
            u.Email.ToLower() == request.Email.ToLower() &&
            u.Password == request.Password &&
            u.Role.ToLower() == request.Role.ToLower());
        if (user is null) return Unauthorized(new { message = "Invalid email, password, or selected role." });
        return Ok(new { user.Id, user.Name, user.Email, user.Role, user.ContactNumber });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Name, email, and password are required." });
        if (request.ContactNumber.Length != 10 || !request.ContactNumber.All(char.IsDigit) ||
            request.ContactNumber[0] < '6' || request.ContactNumber[0] > '9')
            return BadRequest(new { message = "Contact number must be 10 digits and start with 6, 7, 8, or 9." });
        if (await db.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
            return Conflict(new { message = "An account with this email already exists." });

        var user = new UserAccount {
            Name = request.Name.Trim(), Email = request.Email.Trim(),
            Password = request.Password, Role = "Student", ContactNumber = request.ContactNumber
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Ok(new { user.Id, user.Name, user.Email, user.Role, user.ContactNumber });
    }
}
