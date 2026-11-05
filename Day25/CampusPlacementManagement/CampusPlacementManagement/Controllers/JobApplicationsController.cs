
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
    private readonly IJobService _jobService;
    private readonly IAuthorizationService _authorizationService;

    public JobApplicationsController(
        IJobApplicationService applicationService,
        IJobService jobService,
        IAuthorizationService authorizationService)
    {
        _applicationService = applicationService;
        _jobService = jobService;
        _authorizationService = authorizationService;
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

        return Ok(application);
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

    // Company HR views applications for one job
    // GET: api/jobapplications/job/1
    [HttpGet("job/{jobOpeningId}")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> GetApplicationsByJob(int jobOpeningId)
    {
        var job = await _jobService.GetByIdAsync(jobOpeningId);

        if (job == null)
            return NotFound(new { message = "Job not found." });

        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User,
                job.CompanyId,
                "CompanyAccess");

        if (!authorizationResult.Succeeded)
            return Forbid();

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

        // The service filters applications by the HR's own company.
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

        // Get application history with its associated job and company.
        // The service's existing GetApplicationHistoryAsync is not used here.
        // We use the job service after retrieving the application through
        // a new read-only lookup method exposed by the application service.
        var application = await _applicationService
            .GetApplicationForAuthorizationAsync(applicationId);

        if (application == null)
            return NotFound(new { message = "Application not found." });

        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User,
                application.CompanyId,
                "CompanyAccess");

        if (!authorizationResult.Succeeded)
            return Forbid();

        var updatedApplication =
            await _applicationService.UpdateStatusAsync(
                applicationId,
                request,
                userId);

        if (updatedApplication == null)
            return NotFound(new { message = "Application not found." });

        return Ok(updatedApplication);
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

        return Ok(new
        {
            message = "Application withdrawn successfully."
        });
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

        // CompanyHR must pass resource-based authorization.
        if (userRole == "CompanyHR")
        {
            var application = await _applicationService
                .GetApplicationForAuthorizationAsync(applicationId);

            if (application == null)
                return NotFound(new { message = "Application not found." });

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    application.CompanyId,
                    "CompanyAccess");

            if (!authorizationResult.Succeeded)
                return Forbid();
        }

        // Student ownership and SuperAdmin access are still checked
        // inside the service.
        var history =
            await _applicationService.GetApplicationHistoryAsync(
                applicationId,
                userId,
                userRole);

        return Ok(history);
    }
}
