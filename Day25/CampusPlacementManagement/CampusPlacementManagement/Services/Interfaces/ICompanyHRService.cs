
using CampusPlacementManagement.DTOs.CompanyHRs;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICompanyHRService
{
    Task<IEnumerable<CompanyHRResponseDto>> GetAllAsync();

    Task<CompanyHRResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<CompanyHRResponseDto>> GetByCompanyIdAsync(
        int companyId);

    Task<CompanyHRResponseDto> RegisterAsync(
        CreateCompanyHRRequestDto request);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);
}
