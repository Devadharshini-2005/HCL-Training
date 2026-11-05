
using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyHRsController : ControllerBase
{
    private readonly ICompanyHRService _companyHRService;

    public CompanyHRsController(ICompanyHRService companyHRService)
    {
        _companyHRService = companyHRService;
    }

    // GET: api/companyhrs
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var companyHRs = await _companyHRService.GetAllAsync();

        return Ok(companyHRs);
    }

    // GET: api/companyhrs/1
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var companyHR = await _companyHRService.GetByIdAsync(id);

        if (companyHR == null)
            return NotFound(new { message = "Company HR not found." });

        return Ok(companyHR);
    }

    // GET: api/companyhrs/company/1
    [HttpGet("company/{companyId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetByCompanyId(int companyId)
    {
        var companyHRs =
            await _companyHRService.GetByCompanyIdAsync(companyId);

        return Ok(companyHRs);
    }

    // POST: api/companyhrs/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] CreateCompanyHRRequestDto request)
    {
        var companyHR = await _companyHRService.RegisterAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = companyHR.Id },
            companyHR);
    }

    // PUT: api/companyhrs/1/approve
    [HttpPut("{id}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _companyHRService.ApproveAsync(id);

        if (!result)
            return NotFound(new { message = "Company HR not found." });

        return Ok(new { message = "Company HR approved successfully." });
    }

    // PUT: api/companyhrs/1/reject
    [HttpPut("{id}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _companyHRService.RejectAsync(id);

        if (!result)
            return NotFound(new { message = "Company HR not found." });

        return Ok(new { message = "Company HR rejected successfully." });
    }
}
