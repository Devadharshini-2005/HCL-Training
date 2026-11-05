
using CampusPlacementManagement.Authorization.Requirements;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CampusPlacementManagement.Authorization.Handlers;

public class CollegeAccessHandler
    : AuthorizationHandler<CollegeAccessRequirement, int>
{
    private readonly ApplicationDbContext _context;

    public CollegeAccessHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CollegeAccessRequirement requirement,
        int collegeId)
    {
        // SuperAdmin can access every college.
        if (context.User.IsInRole("SuperAdmin"))
        {
            context.Succeed(requirement);
            return;
        }

        // Only Placement Officers can pass the remaining check.
        if (!context.User.IsInRole("PlacementOfficer"))
            return;

        // Get the logged-in user's ID from JWT.
        var userId = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return;

        // Check whether the officer belongs to this college.
        var hasAccess = await _context.PlacementOfficers
            .AnyAsync(officer =>
                officer.UserId == userId &&
                officer.CollegeId == collegeId &&
                officer.User.AccountStatus == AccountStatus.Active &&
                officer.College.Status == CollegeStatus.Approved);

        if (hasAccess)
        {
            context.Succeed(requirement);
        }
    }
}
