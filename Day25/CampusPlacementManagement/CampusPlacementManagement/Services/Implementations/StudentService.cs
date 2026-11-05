
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IPlacementOfficerRepository _officerRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public StudentService(
        IStudentRepository studentRepository,
        IPlacementOfficerRepository officerRepository,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _studentRepository = studentRepository;
        _officerRepository = officerRepository;
        _userManager = userManager;
        _context = context;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();

        return students.Select(MapToResponse);
    }

    public async Task<StudentResponseDto?> GetByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        return student == null ? null : MapToResponse(student);
    }

    public async Task<IEnumerable<StudentResponseDto>> GetByCollegeIdAsync(
        int collegeId)
    {
        var students =
            await _studentRepository.GetByCollegeIdAsync(collegeId);

        return students.Select(MapToResponse);
    }

    public async Task<IEnumerable<StudentResponseDto>> GetPendingAsync(
        int collegeId)
    {
        var students =
            await _studentRepository.GetPendingAsync(collegeId);

        return students.Select(MapToResponse);
    }

    // Enroll a student under the logged-in Placement Officer's college
    public async Task<StudentResponseDto> EnrollAsync(
        CreateStudentRequestDto request,
        string placementOfficerUserId)
    {
        var officer = await _officerRepository
            .GetByUserIdAsync(placementOfficerUserId);

        if (officer == null ||
            officer.User.AccountStatus != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException(
                "Active Placement Officer account not found.");
        }

        if (officer.College.Status != CollegeStatus.Approved)
        {
            throw new InvalidOperationException(
                "Students cannot be enrolled for an unapproved college.");
        }

        if (await _studentRepository.RegisterNumberExistsAsync(
            request.RegisterNumber))
        {
            throw new InvalidOperationException(
                "Register number already exists.");
        }

        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var user = new ApplicationUser
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
                    string.Join("; ",
                        result.Errors.Select(e => e.Description)));
            }

            var student = new Student
            {
                UserId = user.Id,
                CollegeId = officer.CollegeId,
                RegisterNumber = request.RegisterNumber,
                Department = request.Department,
                GraduationYear = request.GraduationYear,
                ResumeUrl = request.ResumeUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _studentRepository.AddAsync(student);

            await transaction.CommitAsync();

            var savedStudent =
                await _studentRepository.GetByIdAsync(student.Id);

            return MapToResponse(savedStudent!);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Approve a student belonging to the logged-in officer's college
    public async Task<bool> ApproveAsync(
        int id,
        string placementOfficerUserId)
    {
        var officer = await _officerRepository
            .GetByUserIdAsync(placementOfficerUserId);

        if (officer == null ||
            officer.User.AccountStatus != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException(
                "Active Placement Officer account not found.");
        }

        var student = await _studentRepository.GetByIdAsync(id);

        if (student == null)
            return false;

        if (student.CollegeId != officer.CollegeId)
        {
            throw new UnauthorizedAccessException(
                "You cannot approve students from another college.");
        }

        if (student.User.AccountStatus != AccountStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending students can be approved.");
        }

        var roleResult = await _userManager.AddToRoleAsync(
            student.User, "Student");

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ",
                    roleResult.Errors.Select(e => e.Description)));
        }

        student.User.AccountStatus = AccountStatus.Active;

        var updateResult =
            await _userManager.UpdateAsync(student.User);

        if (!updateResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ",
                    updateResult.Errors.Select(e => e.Description)));
        }

        await _studentRepository.UpdateAsync(student);

        return true;
    }

    // Reject a student belonging to the logged-in officer's college
    public async Task<bool> RejectAsync(
        int id,
        string placementOfficerUserId)
    {
        var officer = await _officerRepository
            .GetByUserIdAsync(placementOfficerUserId);

        if (officer == null ||
            officer.User.AccountStatus != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException(
                "Active Placement Officer account not found.");
        }

        var student = await _studentRepository.GetByIdAsync(id);

        if (student == null)
            return false;

        if (student.CollegeId != officer.CollegeId)
        {
            throw new UnauthorizedAccessException(
                "You cannot reject students from another college.");
        }

        if (student.User.AccountStatus != AccountStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending students can be rejected.");
        }

        student.User.AccountStatus = AccountStatus.Rejected;

        var result = await _userManager.UpdateAsync(student.User);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ",
                    result.Errors.Select(e => e.Description)));
        }

        return true;
    }

    private static StudentResponseDto MapToResponse(Student student)
    {
        return new StudentResponseDto
        {
            Id = student.Id,
            UserId = student.UserId,
            Email = student.User.Email ?? string.Empty,
            PhoneNumber = student.User.PhoneNumber ?? string.Empty,
            CollegeId = student.CollegeId,
            CollegeName = student.College.CollegeName,
            RegisterNumber = student.RegisterNumber,
            Department = student.Department,
            GraduationYear = student.GraduationYear,
            ResumeUrl = student.ResumeUrl,
            AccountStatus = student.User.AccountStatus.ToString(),
            CreatedAt = student.CreatedAt
        };
    }

    public async Task<IEnumerable<StudentResponseDto>>
    GetPendingStudentsForOfficerAsync(string placementOfficerUserId)
    {
        var officer = await _officerRepository
            .GetByUserIdAsync(placementOfficerUserId);

        if (officer == null ||
            officer.User.AccountStatus != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException(
                "Active Placement Officer account not found.");
        }

        if (officer.College.Status != CollegeStatus.Approved)
        {
            throw new InvalidOperationException(
                "Your college is not approved.");
        }

        var students = await _studentRepository
            .GetPendingAsync(officer.CollegeId);

        return students.Select(MapToResponse);
    }
}
