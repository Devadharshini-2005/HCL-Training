
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    // Get all companies
    public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();

        return companies.Select(MapToResponseDto);
    }

    // Get company by ID
    public async Task<CompanyResponseDto?> GetByIdAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            return null;

        return MapToResponseDto(company);
    }

    // Get pending companies
    public async Task<IEnumerable<CompanyResponseDto>> GetPendingAsync()
    {
        var companies = await _companyRepository.GetPendingAsync();

        return companies.Select(MapToResponseDto);
    }

    // Register company
    public async Task<CompanyResponseDto> RegisterAsync(
        CreateCompanyRequestDto request)
    {
        var emailExists =
            await _companyRepository.EmailExistsAsync(request.Email);

        if (emailExists)
            throw new InvalidOperationException(
                "A company with this email already exists.");

        var company = new Company
        {
            CompanyName = request.CompanyName,
            Description = request.Description,
            Website = request.Website,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Status = CompanyStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _companyRepository.AddAsync(company);

        return MapToResponseDto(company);
    }

    // Update company
    public async Task<CompanyResponseDto?> UpdateAsync(
        int id,
        CreateCompanyRequestDto request)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            return null;

        var emailExists =
            await _companyRepository.EmailExistsAsync(request.Email);

        if (emailExists && company.Email != request.Email)
            throw new InvalidOperationException(
                "Another company is already registered with this email.");

        company.CompanyName = request.CompanyName;
        company.Description = request.Description;
        company.Website = request.Website;
        company.Address = request.Address;
        company.City = request.City;
        company.State = request.State;
        company.Email = request.Email;
        company.PhoneNumber = request.PhoneNumber;
        company.UpdatedAt = DateTime.UtcNow;

        await _companyRepository.UpdateAsync(company);

        return MapToResponseDto(company);
    }

    // Approve company
    public async Task<bool> ApproveAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            return false;

        if (company.Status != CompanyStatus.Pending)
            throw new InvalidOperationException(
                "Only pending companies can be approved.");

        company.Status = CompanyStatus.Approved;
        company.UpdatedAt = DateTime.UtcNow;

        await _companyRepository.UpdateAsync(company);

        return true;
    }

    // Reject company
    public async Task<bool> RejectAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            return false;

        if (company.Status != CompanyStatus.Pending)
            throw new InvalidOperationException(
                "Only pending companies can be rejected.");

        company.Status = CompanyStatus.Rejected;
        company.UpdatedAt = DateTime.UtcNow;

        await _companyRepository.UpdateAsync(company);

        return true;
    }

    // Map entity to response DTO
    private static CompanyResponseDto MapToResponseDto(Company company)
    {
        return new CompanyResponseDto
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Description = company.Description,
            Website = company.Website,
            Address = company.Address,
            City = company.City,
            State = company.State,
            Email = company.Email,
            PhoneNumber = company.PhoneNumber,
            Status = company.Status,
            CreatedAt = company.CreatedAt,
            UpdatedAt = company.UpdatedAt
        };
    }
}
