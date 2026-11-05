
using System.Security.Claims;
using CampusPlacementManagement.DTOs.Jobs;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;
    private readonly IAuthorizationService _authorizationService;

    public JobsController(
        IJobService jobService,
        IAuthorizationService authorizationService)
    {
        _jobService = jobService;
        _authorizationService = authorizationService;
    }

    // Checks whether the logged-in HR can access the job's company.
    private async Task<bool> HasCompanyAccessAsync(int jobId)
    {
        var job = await _jobService.GetByIdAsync(jobId);

        if (job == null)
            return false;

        var result = await _authorizationService.AuthorizeAsync(
            User,
            job.CompanyId,
            "CompanyAccess");

        return result.Succeeded;
    }

    // GET: api/jobs
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _jobService.GetAllAsync();
        return Ok(jobs);
    }

    // GET: api/jobs/5
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _jobService.GetByIdAsync(id);

        if (job == null)
            return NotFound(new { message = "Job not found." });

        return Ok(job);
    }

    // GET: api/jobs/my-company
    [HttpGet("my-company")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> GetMyCompanyJobs()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var jobs = await _jobService.GetByCompanyUserIdAsync(userId);

        return Ok(jobs);
    }

    // GET: api/jobs/published
    [HttpGet("published")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetPublished()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var jobs = await _jobService.GetPublishedAsync(userId);

        return Ok(jobs);
    }

    // POST: api/jobs
    [HttpPost]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> Create(
        [FromBody] CreateJobRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var job = await _jobService.CreateAsync(request, userId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = job.Id },
            job);
    }

    // PUT: api/jobs/5
    [HttpPut("{id}")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateJobRequestDto request)
    {
        if (!await HasCompanyAccessAsync(id))
            return Forbid();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var job = await _jobService.UpdateAsync(id, request, userId);

        if (job == null)
            return NotFound(new { message = "Job not found." });

        return Ok(job);
    }

    // PUT: api/jobs/5/publish
    [HttpPut("{id}/publish")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> Publish(int id)
    {
        if (!await HasCompanyAccessAsync(id))
            return Forbid();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _jobService.PublishAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Job not found." });

        return Ok(new { message = "Job published successfully." });
    }

    // PUT: api/jobs/5/close
    [HttpPut("{id}/close")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> Close(int id)
    {
        if (!await HasCompanyAccessAsync(id))
            return Forbid();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _jobService.CloseAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Job not found." });

        return Ok(new { message = "Job closed successfully." });
    }
}
