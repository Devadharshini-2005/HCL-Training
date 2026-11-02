using WebApiPractice.DTOs;

namespace WebApiPractice.Services;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllDepartmentsAsync();

    Task<DepartmentDto?> GetDepartmentByIdAsync(int id);

    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);

    Task<DepartmentDto?> UpdateDepartmentAsync(int id, CreateDepartmentDto dto);

    Task<DepartmentDeleteResult> DeleteDepartmentAsync(int id);

    Task<PagedResult<DepartmentDto>> GetFilteredAndPagedDepartmentsAsync(
    string? search,
    int pageNumber,
    int pageSize);
}

public enum DepartmentDeleteResult
{
    NotFound,
    HasStudents,
    Deleted
}

