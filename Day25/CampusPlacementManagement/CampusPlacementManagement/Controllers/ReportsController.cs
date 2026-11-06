
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IAuthorizationService _authorizationService;

    public ReportsController(
        IReportService reportService,
        IAuthorizationService authorizationService)
    {
        _reportService = reportService;
        _authorizationService = authorizationService;
    }

    // 1. Platform overview - SuperAdmin
    [HttpGet("platform-overview")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPlatformOverview()
    {
        var report = await _reportService.GetPlatformOverviewAsync();

        return Ok(report);
    }

    // 2. College overview - PlacementOfficer
    [HttpGet("college-overview")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> GetCollegeOverview([FromQuery] int collegeId)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            collegeId,
            "CollegeAccess");

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var report = await _reportService.GetCollegeOverviewAsync(collegeId);

        return Ok(report);
    }

    // 3. Company recruitment - CompanyHR
    [HttpGet("company-recruitment")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> GetCompanyRecruitment([FromQuery] int companyId)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            companyId,
            "CompanyAccess");

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var report = await _reportService.GetCompanyRecruitmentAsync(companyId);

        return Ok(report);
    }

    // 4. Student placement - PlacementOfficer
    [HttpGet("student-placement")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> GetStudentPlacement([FromQuery] int collegeId)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            collegeId,
            "CollegeAccess");

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var report = await _reportService.GetStudentPlacementAsync(collegeId);

        return Ok(report);
    }
}
