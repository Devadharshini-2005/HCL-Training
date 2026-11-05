
using CampusPlacementManagement.DTOs.PlacementOfficers;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IPlacementOfficerService
{
    Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync();

    Task<PlacementOfficerResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<PlacementOfficerResponseDto>> GetByCollegeIdAsync(int collegeId);

    Task<PlacementOfficerResponseDto> RegisterAsync(
        CreatePlacementOfficerRequestDto request);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);
}
