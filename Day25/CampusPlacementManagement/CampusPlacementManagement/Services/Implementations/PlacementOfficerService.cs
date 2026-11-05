
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services.Implementations;

public class PlacementOfficerService : IPlacementOfficerService
{
    private readonly IPlacementOfficerRepository _officerRepository;
    private readonly ICollegeRepository _collegeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ApplicationDbContext _context;

    public PlacementOfficerService(
    IPlacementOfficerRepository officerRepository,
    ICollegeRepository collegeRepository,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context)
    {
        _officerRepository = officerRepository;
        _collegeRepository = collegeRepository;
        _userManager = userManager;
        _context = context;
    }

    // Get all Placement Officers
    public async Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync()
    {
        var officers = await _officerRepository.GetAllAsync();

        return officers.Select(MapToResponseDto);
    }

    // Get Placement Officer by ID
    public async Task<PlacementOfficerResponseDto?> GetByIdAsync(int id)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
            return null;

        return MapToResponseDto(officer);
    }

    // Get Placement Officers by College ID
    public async Task<IEnumerable<PlacementOfficerResponseDto>>
        GetByCollegeIdAsync(int collegeId)
    {
        var officers = await _officerRepository.GetByCollegeIdAsync(collegeId);

        return officers.Select(MapToResponseDto);
    }

    // Register Placement Officer

    public async Task<PlacementOfficerResponseDto> RegisterAsync(
        CreatePlacementOfficerRequestDto request)
    {
        var college = await _collegeRepository.GetByIdAsync(request.CollegeId);

        if (college == null)
            throw new InvalidOperationException("College not found.");

        if (college.Status != CollegeStatus.Approved)
            throw new InvalidOperationException(
                "Placement Officer registration is allowed only for approved colleges.");

        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new InvalidOperationException(
                "An account with this email already exists.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                AccountStatus = AccountStatus.Pending
            };

            var createResult = await _userManager.CreateAsync(
                user, request.Password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        createResult.Errors.Select(e => e.Description)));
            }

            var officer = new PlacementOfficer
            {
                UserId = user.Id,
                CollegeId = request.CollegeId,
                CreatedAt = DateTime.UtcNow
            };

            await _officerRepository.AddAsync(officer);

            await transaction.CommitAsync();

            var savedOfficer =
                await _officerRepository.GetByIdAsync(officer.Id);

            return MapToResponseDto(savedOfficer!);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Approve Placement Officer
    public async Task<bool> ApproveAsync(int id)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
            return false;

        if (officer.User.AccountStatus != AccountStatus.Pending)
            throw new InvalidOperationException(
                "Only pending Placement Officers can be approved.");

        var user = officer.User;

        var roleResult = await _userManager.AddToRoleAsync(
            user, "PlacementOfficer");

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        user.AccountStatus = AccountStatus.Active;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", updateResult.Errors.Select(e => e.Description)));
        }

        return true;
    }

    // Reject Placement Officer
    public async Task<bool> RejectAsync(int id)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
            return false;

        if (officer.User.AccountStatus != AccountStatus.Pending)
            throw new InvalidOperationException(
                "Only pending Placement Officers can be rejected.");

        officer.User.AccountStatus = AccountStatus.Rejected;

        var updateResult = await _userManager.UpdateAsync(officer.User);

        if (!updateResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", updateResult.Errors.Select(e => e.Description)));
        }

        return true;
    }

    // Convert Entity to Response DTO
    private static PlacementOfficerResponseDto MapToResponseDto(
        PlacementOfficer officer)
    {
        return new PlacementOfficerResponseDto
        {
            Id = officer.Id,
            UserId = officer.UserId,
            Email = officer.User.Email ?? string.Empty,
            PhoneNumber = officer.User.PhoneNumber ?? string.Empty,
            CollegeId = officer.CollegeId,
            CollegeName = officer.College.CollegeName,
            AccountStatus = officer.User.AccountStatus.ToString(),
            CreatedAt = officer.CreatedAt
        };
    }
}
