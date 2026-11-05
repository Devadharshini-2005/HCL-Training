
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _collegeRepository;

    public CollegeService(ICollegeRepository collegeRepository)
    {
        _collegeRepository = collegeRepository;
    }

    // Get all colleges
    public async Task<IEnumerable<CollegeResponseDto>> GetAllAsync()
    {
        var colleges = await _collegeRepository.GetAllAsync();

        return colleges.Select(MapToResponseDto);
    }

    // Get college by ID
    public async Task<CollegeResponseDto?> GetByIdAsync(int id)
    {
        var college = await _collegeRepository.GetByIdAsync(id);

        if (college == null)
            return null;

        return MapToResponseDto(college);
    }

    // Get pending colleges
    public async Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync()
    {
        var colleges = await _collegeRepository.GetPendingCollegesAsync();

        return colleges.Select(MapToResponseDto);
    }

    // Register college
    public async Task<CollegeResponseDto> CreateAsync(
        CreateCollegeRequestDto request)
    {
        var collegeCodeExists =
            await _collegeRepository.CollegeCodeExistsAsync(request.CollegeCode);

        if (collegeCodeExists)
        {
            throw new InvalidOperationException(
                "A college with this college code already exists.");
        }

        var college = new College
        {
            CollegeName = request.CollegeName,
            CollegeCode = request.CollegeCode,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Status = CollegeStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _collegeRepository.AddAsync(college);

        return MapToResponseDto(college);
    }

    // Update college
    public async Task<CollegeResponseDto?> UpdateAsync(
        int id,
        UpdateCollegeRequestDto request)
    {
        var college = await _collegeRepository.GetByIdAsync(id);

        if (college == null)
            return null;

        college.CollegeName = request.CollegeName;
        college.Address = request.Address;
        college.City = request.City;
        college.State = request.State;
        college.Email = request.Email;
        college.PhoneNumber = request.PhoneNumber;
        college.UpdatedAt = DateTime.UtcNow;

        await _collegeRepository.UpdateAsync(college);

        return MapToResponseDto(college);
    }

    // Approve college
    public async Task<bool> ApproveAsync(int id)
    {
        var college = await _collegeRepository.GetByIdAsync(id);

        if (college == null)
            return false;

        if (college.Status != CollegeStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending colleges can be approved.");
        }

        college.Status = CollegeStatus.Approved;
        college.UpdatedAt = DateTime.UtcNow;

        await _collegeRepository.UpdateAsync(college);

        return true;
    }

    // Reject college
    public async Task<bool> RejectAsync(int id)
    {
        var college = await _collegeRepository.GetByIdAsync(id);

        if (college == null)
            return false;

        if (college.Status != CollegeStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending colleges can be rejected.");
        }

        college.Status = CollegeStatus.Rejected;
        college.UpdatedAt = DateTime.UtcNow;

        await _collegeRepository.UpdateAsync(college);

        return true;
    }

    // Convert College entity to response DTO
    private static CollegeResponseDto MapToResponseDto(College college)
    {
        return new CollegeResponseDto
        {
            Id = college.Id,
            CollegeName = college.CollegeName,
            CollegeCode = college.CollegeCode,
            Address = college.Address,
            City = college.City,
            State = college.State,
            Email = college.Email,
            PhoneNumber = college.PhoneNumber,
            Status = college.Status,
            CreatedAt = college.CreatedAt,
            UpdatedAt = college.UpdatedAt
        };
    }
}
