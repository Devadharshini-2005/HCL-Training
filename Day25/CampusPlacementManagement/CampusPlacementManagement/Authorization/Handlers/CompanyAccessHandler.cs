
using CampusPlacementManagement.Authorization.Requirements;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CampusPlacementManagement.Authorization.Handlers;

public class CompanyAccessHandler
    : AuthorizationHandler<CompanyAccessRequirement, int>
{
    private readonly ApplicationDbContext _context;

    public CompanyAccessHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CompanyAccessRequirement requirement,
        int companyId)
    {
        // SuperAdmin can access every company.
        if (context.User.IsInRole("SuperAdmin"))
        {
            context.Succeed(requirement);
            return;
        }

        // Only Company HR can pass the remaining check.
        if (!context.User.IsInRole("CompanyHR"))
            return;

        // Get logged-in user's ID from JWT.
        var userId = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return;

        // Check whether the HR belongs to this company.
        var hasAccess = await _context.CompanyHRs
            .AnyAsync(hr =>
                hr.UserId == userId &&
                hr.CompanyId == companyId &&
                hr.User.AccountStatus == AccountStatus.Active &&
                hr.Company.Status == CompanyStatus.Approved);

        if (hasAccess)
        {
            context.Succeed(requirement);
        }
    }
}
