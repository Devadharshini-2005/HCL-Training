
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacementOfficersController : ControllerBase
{
    private readonly IPlacementOfficerService _officerService;

    public PlacementOfficersController(
        IPlacementOfficerService officerService)
    {
        _officerService = officerService;
    }

    // GET: api/placementofficers
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var officers = await _officerService.GetAllAsync();

        return Ok(officers);
    }

    // GET: api/placementofficers/1
    [HttpGet("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var officer = await _officerService.GetByIdAsync(id);

        if (officer == null)
            return NotFound(new { message = "Placement Officer not found." });

        return Ok(officer);
    }

    // GET: api/placementofficers/college/1
    [HttpGet("college/{collegeId:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetByCollegeId(int collegeId)
    {
        var officers = await _officerService.GetByCollegeIdAsync(collegeId);

        return Ok(officers);
    }

    // POST: api/placementofficers/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        CreatePlacementOfficerRequestDto request)
    {
        var officer = await _officerService.RegisterAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = officer.Id },
            officer);
    }

    // PUT: api/placementofficers/1/approve
    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _officerService.ApproveAsync(id);

        if (!result)
            return NotFound(new { message = "Placement Officer not found." });

        return Ok(new
        {
            message = "Placement Officer approved successfully."
        });
    }

    // PUT: api/placementofficers/1/reject
    [HttpPut("{id:int}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _officerService.RejectAsync(id);

        if (!result)
            return NotFound(new { message = "Placement Officer not found." });

        return Ok(new
        {
            message = "Placement Officer rejected successfully."
        });
    }
}
