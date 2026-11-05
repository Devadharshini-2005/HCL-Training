
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    // GET: api/companies
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _companyService.GetAllAsync();

        return Ok(companies);
    }

    // GET: api/companies/1
    [HttpGet("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _companyService.GetByIdAsync(id);

        if (company == null)
            return NotFound(new { message = "Company not found." });

        return Ok(company);
    }

    // GET: api/companies/pending
    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var companies = await _companyService.GetPendingAsync();

        return Ok(companies);
    }

    // POST: api/companies
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        CreateCompanyRequestDto request)
    {
        var company = await _companyService.RegisterAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = company.Id },
            company);
    }

    // PUT: api/companies/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Update(
        int id,
        CreateCompanyRequestDto request)
    {
        var company = await _companyService.UpdateAsync(id, request);

        if (company == null)
            return NotFound(new { message = "Company not found." });

        return Ok(company);
    }

    // PUT: api/companies/1/approve
    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _companyService.ApproveAsync(id);

        if (!result)
            return NotFound(new { message = "Company not found." });

        return Ok(new
        {
            message = "Company approved successfully."
        });
    }

    // PUT: api/companies/1/reject
    [HttpPut("{id:int}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _companyService.RejectAsync(id);

        if (!result)
            return NotFound(new { message = "Company not found." });

        return Ok(new
        {
            message = "Company rejected successfully."
        });
    }
}
