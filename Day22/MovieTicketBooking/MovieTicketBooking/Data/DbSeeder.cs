using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services;

namespace MovieTicketBooking.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(
        ApplicationDbContext context,
        PasswordService passwordService)
    {
        // Check whether Admin already exists
        bool adminExists = await context.Users
            .AnyAsync(u => u.Email == "admin@movieticket.com");

        if (adminExists)
        {
            return;
        }

        // Find Admin role
        var adminRole = await context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == "Admin");

        if (adminRole == null)
        {
            throw new InvalidOperationException(
                "Admin role not found. Apply database migrations first.");
        }

        // Create Admin user
        var admin = new User
        {
            FullName = "Movie Admin",
            Email = "admin@movieticket.com",
            PhoneNumber = "9876543210",
            RoleId = adminRole.RoleId
        };

        // Hash password
        admin.PasswordHash = passwordService.HashPassword(
            admin,
            "Admin@123"
        );

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}