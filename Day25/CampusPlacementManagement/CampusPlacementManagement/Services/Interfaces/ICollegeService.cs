
using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICollegeService
{
    Task<IEnumerable<CollegeResponseDto>> GetAllAsync();

    Task<CollegeResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync();

    Task<CollegeResponseDto> CreateAsync(CreateCollegeRequestDto request);

    Task<CollegeResponseDto?> UpdateAsync(
        int id,
        UpdateCollegeRequestDto request);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);
}
