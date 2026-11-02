using Microsoft.AspNetCore.Mvc;
using WebApiPractice.DTOs;
using WebApiPractice.Services;

namespace WebApiPractice.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();

        return Ok(departments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await _departmentService.GetDepartmentByIdAsync(id);

        if (department is null)
            return NotFound(new { message = "Department not found." });

        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment(
        [FromBody] CreateDepartmentDto dto)
    {
        try
        {
            var department = await _departmentService.CreateDepartmentAsync(dto);

            return CreatedAtAction(
                nameof(GetDepartmentById),
                new { id = department.DepartmentId },
                department);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDepartment(
        int id,
        [FromBody] CreateDepartmentDto dto)
    {
        try
        {
            var department = await _departmentService.UpdateDepartmentAsync(id, dto);

            if (department is null)
                return NotFound(new { message = "Department not found." });

            return Ok(department);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var result = await _departmentService.DeleteDepartmentAsync(id);

        return result switch
        {
            DepartmentDeleteResult.NotFound =>
                NotFound(new { message = "Department not found." }),

            DepartmentDeleteResult.HasStudents =>
                Conflict(new
                {
                    message = "Cannot delete a department that has students assigned to it."
                }),

            DepartmentDeleteResult.Deleted =>
                NoContent(),

            _ => StatusCode(500)
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments(
    [FromQuery] string? search,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new
            {
                message = "Page number must be greater than or equal to 1."
            });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                message = "Page size must be between 1 and 100."
            });
        }

        var departments =
            await _departmentService.GetFilteredAndPagedDepartmentsAsync(
                search,
                pageNumber,
                pageSize);

        return Ok(departments);
    }

}