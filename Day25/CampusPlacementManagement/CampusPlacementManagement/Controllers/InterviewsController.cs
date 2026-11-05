
using System.Security.Claims;
using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class InterviewsController : ControllerBase
{
    private readonly IInterviewScheduleService _interviewScheduleService;

    public InterviewsController(
        IInterviewScheduleService interviewScheduleService)
    {
        _interviewScheduleService = interviewScheduleService;
    }

    // POST: api/applications/{applicationId}/interviews
    [HttpPost("applications/{applicationId:int}/interviews")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> Create(
        int applicationId,
        [FromBody] CreateInterviewScheduleRequestDto request)
    {
        var companyHRUserId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(companyHRUserId))
            return Unauthorized();

        var interview = await _interviewScheduleService.CreateAsync(
            applicationId,
            request,
            companyHRUserId);

        return StatusCode(201, interview);
    }

    // GET: api/applications/{applicationId}/interviews
    [HttpGet("applications/{applicationId:int}/interviews")]
    [Authorize(Roles = "SuperAdmin,CompanyHR,PlacementOfficer,Student")]
    public async Task<IActionResult> GetByApplicationId(int applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) ||
            string.IsNullOrEmpty(userRole))
        {
            return Unauthorized();
        }

        var interviews =
            await _interviewScheduleService.GetByApplicationIdAsync(
                applicationId,
                userId,
                userRole);

        return Ok(interviews);
    }

    // PUT: api/interviews/{id}
    [HttpPut("interviews/{id:int}")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateInterviewScheduleRequestDto request)
    {
        var companyHRUserId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(companyHRUserId))
            return Unauthorized();

        var interview = await _interviewScheduleService.UpdateAsync(
            id,
            request,
            companyHRUserId);

        return Ok(interview);
    }

    // PUT: api/interviews/{id}/feedback
    [HttpPut("interviews/{id:int}/feedback")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> UpdateFeedback(
        int id,
        [FromBody] UpdateInterviewFeedbackRequestDto request)
    {
        var companyHRUserId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(companyHRUserId))
            return Unauthorized();

        var interview =
            await _interviewScheduleService.UpdateFeedbackAsync(
                id,
                request,
                companyHRUserId);

        return Ok(interview);
    }
}
