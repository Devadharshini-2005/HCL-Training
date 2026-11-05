using System.Security.Claims;
using CampusPlacementManagement.DTOs.Applications;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _applicationService;

    public JobApplicationsController(
        IJobApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    // Student applies for a job
    // POST: api/jobapplications/apply/1
    [HttpPost("apply/{jobOpeningId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Apply(int jobOpeningId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var application = await _applicationService.ApplyAsync(
            jobOpeningId,
            userId);

        return CreatedAtAction(
            nameof(GetMyApplications),
            application);
    }

    // Student views their own applications
    // GET: api/jobapplications/my-applications
    [HttpGet("my-applications")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyApplications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var applications =
            await _applicationService.GetMyApplicationsAsync(userId);

        return Ok(applications);
    }

    // Company HR views applications for a particular job
    // GET: api/jobapplications/job/1
    [HttpGet("job/{jobOpeningId}")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> GetApplicationsByJob(int jobOpeningId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var applications =
            await _applicationService.GetApplicationsByJobAsync(
                jobOpeningId,
                userId);

        return Ok(applications);
    }

    // Company HR views all applications for their company
    // GET: api/jobapplications/company
    [HttpGet("company")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> GetApplicationsByCompany()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var applications =
            await _applicationService.GetApplicationsByCompanyAsync(userId);

        return Ok(applications);
    }

    // Company HR updates application status
    // PUT: api/jobapplications/1/status
    [HttpPut("{applicationId}/status")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> UpdateStatus(
        int applicationId,
        UpdateJobApplicationStatusRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var application = await _applicationService.UpdateStatusAsync(
            applicationId,
            request,
            userId);

        if (application == null)
            return NotFound(new { message = "Application not found." });

        return Ok(application);
    }

    // Student withdraws their own application
    // PUT: api/jobapplications/1/withdraw
    [HttpPut("{applicationId}/withdraw")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Withdraw(int applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _applicationService.WithdrawAsync(
            applicationId,
            userId);

        if (!result)
            return NotFound(new { message = "Application not found." });

        return Ok(new { message = "Application withdrawn successfully." });
    }

    // View application status history
    // GET: api/jobapplications/1/history
    [HttpGet("{applicationId}/history")]
    [Authorize(Roles = "Student,CompanyHR,SuperAdmin")]
    public async Task<IActionResult> GetApplicationHistory(int applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) ||
            string.IsNullOrEmpty(userRole))
            return Unauthorized();

        var history =
            await _applicationService.GetApplicationHistoryAsync(
                applicationId,
                userId,
                userRole);

        return Ok(history);
    }
}