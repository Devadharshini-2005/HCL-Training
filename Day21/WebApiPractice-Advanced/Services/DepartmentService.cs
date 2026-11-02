using WebApiPractice.DTOs;
using WebApiPractice.Models;
using WebApiPractice.Repositories;

namespace WebApiPractice.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _repository.GetAllAsync();

        return departments.Select(d => new DepartmentDto
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName,
            StudentCount = d.Students.Count
        }).ToList();
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department is null)
            return null;

        return new DepartmentDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            StudentCount = department.Students.Count
        };
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(
        CreateDepartmentDto dto)
    {
        var departmentName = dto.DepartmentName.Trim();

        if (await _repository.ExistsByNameAsync(departmentName))
        {
            throw new InvalidOperationException(
                "A department with this name already exists.");
        }

        var department = new Department
        {
            DepartmentName = departmentName
        };

        await _repository.AddAsync(department);

        return new DepartmentDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            StudentCount = 0
        };
    }

    public async Task<DepartmentDto?> UpdateDepartmentAsync(
        int id,
        CreateDepartmentDto dto)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department is null)
            return null;

        var departmentName = dto.DepartmentName.Trim();

        if (!string.Equals(
                department.DepartmentName,
                departmentName,
                StringComparison.OrdinalIgnoreCase)
            && await _repository.ExistsByNameAsync(departmentName))
        {
            throw new InvalidOperationException(
                "A department with this name already exists.");
        }

        department.DepartmentName = departmentName;

        await _repository.UpdateAsync(department);

        return new DepartmentDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            StudentCount = department.Students.Count
        };
    }

    public async Task<DepartmentDeleteResult> DeleteDepartmentAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department is null)
            return DepartmentDeleteResult.NotFound;

        if (department.Students.Any())
            return DepartmentDeleteResult.HasStudents;

        await _repository.DeleteAsync(department);

        return DepartmentDeleteResult.Deleted;
    }

    public async Task<PagedResult<DepartmentDto>> GetFilteredAndPagedDepartmentsAsync(
    string? search,
    int pageNumber,
    int pageSize)
    {
        var departments = await _repository.GetFilteredAndPagedAsync(
            search,
            pageNumber,
            pageSize);

        var totalCount = await _repository.GetFilteredCountAsync(search);

        var departmentDtos = departments.Select(d => new DepartmentDto
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName,
            StudentCount = d.Students.Count
        }).ToList();

        return new PagedResult<DepartmentDto>
        {
            Items = departmentDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}