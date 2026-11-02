using Microsoft.AspNetCore.Mvc;
using WebApiPractice.DTOs;
using WebApiPractice.Services;

namespace WebApiPractice.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();

        return Ok(students);
    }

    // GET: api/students/1
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var student = await _studentService.GetStudentByIdAsync(id);

        if (student is null)
        {
            return NotFound(new
            {
                message = "Student not found."
            });
        }

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<StudentDto>> AddStudent(
        CreateStudentDto studentDto)
    {
        if (await _studentService.EmailExistsAsync(studentDto.Email))
        {
            return Conflict(new
            {
                message = "A student with this email already exists."
            });
        }

        var createdStudent =
            await _studentService.AddStudentAsync(studentDto);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = createdStudent.Id },
            createdStudent);
    }

    // PUT: api/students/1
    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDto>> UpdateStudent(
        int id,
        UpdateStudentDto studentDto)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        if (await _studentService.EmailExistsAsync(studentDto.Email, id))
        {
            return Conflict(new
            {
                message = "Another student with this email already exists."
            });
        }

        var updatedStudent =
            await _studentService.UpdateStudentAsync(id, studentDto);

        if (updatedStudent is null)
        {
            return NotFound(new
            {
                message = "Student not found."
            });
        }

        return Ok(updatedStudent);
    }

    // DELETE: api/students/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var deleted = await _studentService.DeleteStudentAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Student not found."
            });
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<StudentDto>>> SearchStudents(
    [FromQuery] string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest("Search term is required.");

        var students = await _studentService.SearchStudentsByNameAsync(key);

        return Ok(students);
    }

    [HttpGet("department")]
    public async Task<ActionResult<List<StudentDto>>> GetStudentsByDepartment(
    [FromQuery] int departmentId)
    {
        if(departmentId <= 0)
            return BadRequest("Department ID must be greater than zero.");

        var students = await _studentService
            .GetStudentsByDepartmentIdAsync(departmentId);

        return Ok(students);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<StudentDto>>> GetPagedStudents(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
            return BadRequest("Page number must be at least 1.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Page size must be between 1 and 100.");

        var result = await _studentService.GetPagedStudentsAsync(
            pageNumber,
            pageSize);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StudentDto>>> GetStudents(
    [FromQuery] string? search = null,
    [FromQuery] int? departmentId = null,
    [FromQuery] string? sortBy = "id",
    [FromQuery] string? sortOrder = "asc",
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        // Validate pagination
        if (pageNumber < 1)
            return BadRequest("Page number must be at least 1.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Page size must be between 1 and 100.");

        // Normalize sorting parameters
        sortBy = string.IsNullOrWhiteSpace(sortBy)
            ? "id"
            : sortBy.Trim().ToLower();

        sortOrder = string.IsNullOrWhiteSpace(sortOrder)
            ? "asc"
            : sortOrder.Trim().ToLower();

        // Validate sorting
        if (sortBy != "id" && sortBy != "name" && sortBy != "age")
            return BadRequest("sortBy must be id, name, or age.");

        if (sortOrder != "asc" && sortOrder != "desc")
            return BadRequest("sortOrder must be asc or desc.");

        if (departmentId.HasValue && departmentId.Value < 1)
            return BadRequest("Department ID must be greater than 0.");

        var result = await _studentService.GetFilteredAndPagedStudentsAsync(
            search,
            departmentId,
            sortBy,
            sortOrder,
            pageNumber,
            pageSize);

        return Ok(result);
    }
}