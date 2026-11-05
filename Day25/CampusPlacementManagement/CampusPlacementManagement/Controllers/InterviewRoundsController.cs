
using System.Security.Claims;
using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/jobs/{jobId:int}/interview-rounds")]
[Authorize]
public class InterviewRoundsController : ControllerBase
{
    private readonly IInterviewRoundService _interviewRoundService;

    public InterviewRoundsController(
        IInterviewRoundService interviewRoundService)
    {
        _interviewRoundService = interviewRoundService;
    }

    // GET: api/jobs/2/interview-rounds
    [HttpGet]
    public async Task<IActionResult> GetByJobOpeningId(int jobId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId) ||
            string.IsNullOrEmpty(userRole))
        {
            return Unauthorized();
        }

        var rounds = await _interviewRoundService
            .GetByJobOpeningIdAsync(jobId, userId, userRole);

        return Ok(rounds);
    }

    // POST: api/jobs/2/interview-rounds
    [Authorize(Roles = "CompanyHR")]
    [HttpPost]
    public async Task<IActionResult> Create(
        int jobId,
        CreateInterviewRoundRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var round = await _interviewRoundService.CreateAsync(
            jobId,
            request,
            userId);

        return CreatedAtAction(
            nameof(GetByJobOpeningId),
            new { jobId },
            round);
    }
}
