
using CampusPlacementManagement.DTOs.Companies;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyResponseDto>> GetAllAsync();

    Task<CompanyResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<CompanyResponseDto>> GetPendingAsync();

    Task<CompanyResponseDto> RegisterAsync(
        CreateCompanyRequestDto request);

    Task<CompanyResponseDto?> UpdateAsync(
        int id,
        CreateCompanyRequestDto request);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);
}
