
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class CompanyHRService : ICompanyHRService
{
    private readonly ICompanyHRRepository _companyHRRepository;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Models.Identity.ApplicationUser> _userManager;

    public CompanyHRService(
        ICompanyHRRepository companyHRRepository,
        ApplicationDbContext context,
        UserManager<Models.Identity.ApplicationUser> userManager)
    {
        _companyHRRepository = companyHRRepository;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IEnumerable<CompanyHRResponseDto>> GetAllAsync()
    {
        var companyHRs = await _companyHRRepository.GetAllAsync();

        return companyHRs.Select(MapToResponse);
    }

    public async Task<CompanyHRResponseDto?> GetByIdAsync(int id)
    {
        var companyHR = await _companyHRRepository.GetByIdAsync(id);

        return companyHR == null ? null : MapToResponse(companyHR);
    }

    public async Task<IEnumerable<CompanyHRResponseDto>> GetByCompanyIdAsync(
        int companyId)
    {
        var companyHRs =
            await _companyHRRepository.GetByCompanyIdAsync(companyId);

        return companyHRs.Select(MapToResponse);
    }

    public async Task<CompanyHRResponseDto> RegisterAsync(
        CreateCompanyHRRequestDto request)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId);

        if (company == null)
            throw new KeyNotFoundException("Company not found.");

        if (company.Status != CompanyStatus.Approved)
            throw new InvalidOperationException(
                "Only approved companies can register HR accounts.");

        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new InvalidOperationException(
                "An account with this email already exists.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var user = new Models.Identity.ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(
                user, request.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            var companyHR = new CompanyHR
            {
                UserId = user.Id,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow
            };

            await _companyHRRepository.AddAsync(companyHR);

            await transaction.CommitAsync();

            var savedHR =
                await _companyHRRepository.GetByIdAsync(companyHR.Id);

            return MapToResponse(savedHR!);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var companyHR = await _companyHRRepository.GetByIdAsync(id);

        if (companyHR == null)
            return false;

        if (companyHR.User.AccountStatus != AccountStatus.Pending)
            throw new InvalidOperationException(
                "Only pending Company HR accounts can be approved.");

        var roleResult = await _userManager.AddToRoleAsync(
            companyHR.User, "CompanyHR");

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ", roleResult.Errors.Select(e => e.Description)));
        }

        companyHR.User.AccountStatus = AccountStatus.Active;

        var updateResult = await _userManager.UpdateAsync(companyHR.User);

        if (!updateResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ", updateResult.Errors.Select(e => e.Description)));
        }

        await _companyHRRepository.UpdateAsync(companyHR);

        return true;
    }

    public async Task<bool> RejectAsync(int id)
    {
        var companyHR = await _companyHRRepository.GetByIdAsync(id);

        if (companyHR == null)
            return false;

        if (companyHR.User.AccountStatus != AccountStatus.Pending)
            throw new InvalidOperationException(
                "Only pending Company HR accounts can be rejected.");

        companyHR.User.AccountStatus = AccountStatus.Rejected;

        var result = await _userManager.UpdateAsync(companyHR.User);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        return true;
    }

    private static CompanyHRResponseDto MapToResponse(
        CompanyHR companyHR)
    {
        return new CompanyHRResponseDto
        {
            Id = companyHR.Id,
            UserId = companyHR.UserId,
            Email = companyHR.User.Email ?? string.Empty,
            PhoneNumber = companyHR.User.PhoneNumber ?? string.Empty,
            CompanyId = companyHR.CompanyId,
            CompanyName = companyHR.Company.CompanyName,
            AccountStatus = companyHR.User.AccountStatus.ToString(),
            CreatedAt = companyHR.CreatedAt
        };
    }
}
