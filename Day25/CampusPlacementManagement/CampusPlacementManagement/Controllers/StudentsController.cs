
using System.Security.Claims;
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/students
    // SuperAdmin can view all students
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();

        return Ok(students);
    }

    // GET: api/students/1
    // SuperAdmin can view a student by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetByIdAsync(id);

        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    // GET: api/students/college/1
    [HttpGet("college/{collegeId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetByCollegeId(int collegeId)
    {
        var students =
            await _studentService.GetByCollegeIdAsync(collegeId);

        return Ok(students);
    }

    // GET: api/students/pending
    // Placement Officer can view pending students in their own college
    [HttpGet("pending")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> GetPending()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var officerStudents =
            await _studentService.GetPendingStudentsForOfficerAsync(userId);

        return Ok(officerStudents);
    }

    // POST: api/students/enroll
    [HttpPost("enroll")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> Enroll(
        [FromBody] CreateStudentRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var student = await _studentService.EnrollAsync(request, userId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.Id },
            student);
    }

    // PUT: api/students/1/approve
    [HttpPut("{id}/approve")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> Approve(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _studentService.ApproveAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Student not found." });

        return Ok(new { message = "Student approved successfully." });
    }

    // PUT: api/students/1/reject
    [HttpPut("{id}/reject")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> Reject(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _studentService.RejectAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Student not found." });

        return Ok(new { message = "Student rejected successfully." });
    }
}
