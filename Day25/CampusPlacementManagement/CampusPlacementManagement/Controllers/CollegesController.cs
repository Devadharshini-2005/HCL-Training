
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollegesController : ControllerBase
{
    private readonly ICollegeService _collegeService;

    public CollegesController(ICollegeService collegeService)
    {
        _collegeService = collegeService;
    }

    // GET: api/colleges
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var colleges = await _collegeService.GetAllAsync();

        return Ok(colleges);
    }

    // GET: api/colleges/5
    [HttpGet("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var college = await _collegeService.GetByIdAsync(id);

        if (college == null)
            return NotFound(new { message = "College not found." });

        return Ok(college);
    }

    // GET: api/colleges/pending
    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var colleges = await _collegeService.GetPendingCollegesAsync();

        return Ok(colleges);
    }

    // POST: api/colleges
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(
        CreateCollegeRequestDto request)
    {
        var college = await _collegeService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = college.Id },
            college);
    }

    // PUT: api/colleges/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCollegeRequestDto request)
    {
        var college = await _collegeService.UpdateAsync(id, request);

        if (college == null)
            return NotFound(new { message = "College not found." });

        return Ok(college);
    }

    // PUT: api/colleges/5/approve
    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _collegeService.ApproveAsync(id);

        if (!result)
            return NotFound(new { message = "College not found." });

        return Ok(new { message = "College approved successfully." });
    }

    // PUT: api/colleges/5/reject
    [HttpPut("{id:int}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _collegeService.RejectAsync(id);

        if (!result)
            return NotFound(new { message = "College not found." });

        return Ok(new { message = "College rejected successfully." });
    }
}
