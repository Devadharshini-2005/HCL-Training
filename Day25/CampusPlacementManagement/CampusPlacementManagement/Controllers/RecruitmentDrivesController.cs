
using System.Security.Claims;
using CampusPlacementManagement.DTOs.RecruitmentDrives;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/recruitment-drives")]
[Authorize]
public class RecruitmentDrivesController : ControllerBase
{
    private readonly IRecruitmentDriveService _recruitmentDriveService;

    public RecruitmentDrivesController(
        IRecruitmentDriveService recruitmentDriveService)
    {
        _recruitmentDriveService = recruitmentDriveService;
    }

    // GET: api/recruitment-drives
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) ||
            string.IsNullOrEmpty(userRole))
        {
            return Unauthorized();
        }

        var drives = await _recruitmentDriveService.GetAllAsync(
            userId,
            userRole);

        return Ok(drives);
    }

    // GET: api/recruitment-drives/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) ||
            string.IsNullOrEmpty(userRole))
        {
            return Unauthorized();
        }

        var drive = await _recruitmentDriveService.GetByIdAsync(
            id,
            userId,
            userRole);

        if (drive == null)
        {
            return NotFound(new
            {
                message = "Recruitment drive not found."
            });
        }

        return Ok(drive);
    }

    // POST: api/recruitment-drives
    [Authorize(Roles = "CompanyHR")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRecruitmentDriveRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var drive = await _recruitmentDriveService.CreateAsync(
            request,
            userId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = drive.Id },
            drive);
    }

    // PUT: api/recruitment-drives/1
    [Authorize(Roles = "CompanyHR")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateRecruitmentDriveRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var drive = await _recruitmentDriveService.UpdateAsync(
            id,
            request,
            userId);

        return Ok(drive);
    }

    // PUT: api/recruitment-drives/1/cancel
    [Authorize(Roles = "CompanyHR")]
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _recruitmentDriveService.CancelAsync(id, userId);

        return Ok(new
        {
            message = "Recruitment drive cancelled successfully."
        });
    }
}
