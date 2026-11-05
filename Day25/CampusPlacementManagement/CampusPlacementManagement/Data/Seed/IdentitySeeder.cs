using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            "SuperAdmin",
            "PlacementOfficer",
            "CompanyHR",
            "Student"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role {role}: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    public static async Task SeedSuperAdminAsync(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        // Read credentials from User Secrets
        var email = configuration["SuperAdmin:Email"];
        var password = configuration["SuperAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "SuperAdmin email or password is missing from User Secrets.");
        }

        // Check whether SuperAdmin already exists
        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            if (!await userManager.IsInRoleAsync(existingUser, "SuperAdmin"))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    existingUser, "SuperAdmin");

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Failed to assign SuperAdmin role.");
                }
            }

            return;
        }

        // Create the first SuperAdmin user
        var superAdmin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            AccountStatus = AccountStatus.Active
        };

        var createResult = await userManager.CreateAsync(
            superAdmin, password);

        if (!createResult.Succeeded)
        {
            throw new Exception(
                "Failed to create SuperAdmin: " +
                string.Join(", ",
                    createResult.Errors.Select(e => e.Description)));
        }

        // Assign SuperAdmin role
        var addRoleResult = await userManager.AddToRoleAsync(
            superAdmin, "SuperAdmin");

        if (!addRoleResult.Succeeded)
        {
            throw new Exception("Failed to assign SuperAdmin role.");
        }
    }
}