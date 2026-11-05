
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.RecruitmentDrives;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class RecruitmentDriveService : IRecruitmentDriveService
{
    private readonly ApplicationDbContext _context;

    public RecruitmentDriveService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecruitmentDriveResponseDto>> GetAllAsync(
        string userId,
        string userRole)
    {
        var query = _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .AsQueryable();

        if (userRole == "SuperAdmin")
        {
            // SuperAdmin can view all drives.
        }
        else if (userRole == "CompanyHR")
        {
            var hr = await GetActiveCompanyHRAsync(userId);

            query = query.Where(d =>
                d.JobOpening.CompanyId == hr.CompanyId);
        }
        else if (userRole == "Student")
        {
            var student = await _context.Students
                .Include(s => s.College)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null ||
                student.User.AccountStatus != AccountStatus.Active ||
                student.College.Status != CollegeStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to view recruitment drives.");
            }

            query = query.Where(d =>
                d.CollegeId == student.CollegeId);
        }
        else if (userRole == "PlacementOfficer")
        {
            var officer = await _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (officer == null ||
                officer.User.AccountStatus != AccountStatus.Active ||
                officer.College.Status != CollegeStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to view recruitment drives.");
            }

            query = query.Where(d =>
                d.CollegeId == officer.CollegeId);
        }
        else
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to view recruitment drives.");
        }

        var drives = await query
            .AsNoTracking()
            .ToListAsync();

        return drives.Select(MapToResponse).ToList();
    }

    public async Task<RecruitmentDriveResponseDto?> GetByIdAsync(
        int id,
        string userId,
        string userRole)
    {
        var drive = await _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (drive == null)
        {
            return null;
        }

        if (userRole == "SuperAdmin")
        {
            return MapToResponse(drive);
        }

        if (userRole == "CompanyHR")
        {
            var hr = await GetActiveCompanyHRAsync(userId);

            if (drive.JobOpening.CompanyId != hr.CompanyId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot access another company's recruitment drive.");
            }
        }
        else if (userRole == "Student")
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null ||
                student.User.AccountStatus != AccountStatus.Active ||
                student.College.Status != CollegeStatus.Approved ||
                drive.CollegeId != student.CollegeId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot access this recruitment drive.");
            }
        }
        else if (userRole == "PlacementOfficer")
        {
            var officer = await _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (officer == null ||
                officer.User.AccountStatus != AccountStatus.Active ||
                officer.College.Status != CollegeStatus.Approved ||
                drive.CollegeId != officer.CollegeId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot access this recruitment drive.");
            }
        }
        else
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to view this recruitment drive.");
        }

        return MapToResponse(drive);
    }

    public async Task<RecruitmentDriveResponseDto> CreateAsync(
        CreateRecruitmentDriveRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var job = await _context.JobOpenings
            .FirstOrDefaultAsync(j => j.Id == request.JobOpeningId);

        if (job == null)
        {
            throw new KeyNotFoundException("Job opening not found.");
        }

        if (job.CompanyId != hr.CompanyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot create a drive for another company's job.");
        }

        if (job.Status != JobStatus.Published)
        {
            throw new InvalidOperationException(
                "A recruitment drive can only be created for a published job.");
        }

        var college = await _context.Colleges
            .FirstOrDefaultAsync(c => c.Id == request.CollegeId);

        if (college == null)
        {
            throw new KeyNotFoundException("College not found.");
        }

        if (college.Status != CollegeStatus.Approved)
        {
            throw new InvalidOperationException(
                "A recruitment drive can only be created for an approved college.");
        }

        var isTargetCollege = await _context.JobTargetColleges
            .AnyAsync(t =>
                t.JobOpeningId == job.Id &&
                t.CollegeId == college.Id);

        if (!isTargetCollege)
        {
            throw new InvalidOperationException(
                "This college is not targeted by the selected job.");
        }

        if (request.DriveDate <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Recruitment drive date must be in the future.");
        }

        var drive = new RecruitmentDrive
        {
            JobOpeningId = request.JobOpeningId,
            CollegeId = request.CollegeId,
            DriveName = request.DriveName,
            DriveDate = request.DriveDate,
            Venue = request.Venue,
            Description = request.Description,
            Status = RecruitmentDriveStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        await _context.RecruitmentDrives.AddAsync(drive);
        await _context.SaveChangesAsync();

        var createdDrive = await _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .FirstAsync(d => d.Id == drive.Id);

        return MapToResponse(createdDrive);
    }

    public async Task<RecruitmentDriveResponseDto> UpdateAsync(
        int id,
        UpdateRecruitmentDriveRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var drive = await _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .Include(d => d.College)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (drive == null)
        {
            throw new KeyNotFoundException("Recruitment drive not found.");
        }

        if (drive.JobOpening.CompanyId != hr.CompanyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot update another company's recruitment drive.");
        }

        if (drive.Status != RecruitmentDriveStatus.Scheduled)
        {
            throw new InvalidOperationException(
                "Only scheduled recruitment drives can be updated.");
        }

        if (request.DriveDate <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Recruitment drive date must be in the future.");
        }

        drive.DriveName = request.DriveName;
        drive.DriveDate = request.DriveDate;
        drive.Venue = request.Venue;
        drive.Description = request.Description;

        await _context.SaveChangesAsync();

        return MapToResponse(drive);
    }

    public async Task CancelAsync(
        int id,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var drive = await _context.RecruitmentDrives
            .Include(d => d.JobOpening)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (drive == null)
        {
            throw new KeyNotFoundException("Recruitment drive not found.");
        }

        if (drive.JobOpening.CompanyId != hr.CompanyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot cancel another company's recruitment drive.");
        }

        if (drive.Status != RecruitmentDriveStatus.Scheduled)
        {
            throw new InvalidOperationException(
                "Only scheduled recruitment drives can be cancelled.");
        }

        drive.Status = RecruitmentDriveStatus.Cancelled;

        await _context.SaveChangesAsync();
    }

    private async Task<CompanyHR> GetActiveCompanyHRAsync(string userId)
    {
        var hr = await _context.CompanyHRs
            .Include(h => h.User)
            .Include(h => h.Company)
            .FirstOrDefaultAsync(h => h.UserId == userId);

        if (hr == null ||
            hr.User.AccountStatus != AccountStatus.Active ||
            hr.Company.Status != CompanyStatus.Approved)
        {
            throw new UnauthorizedAccessException(
                "Active CompanyHR access is required.");
        }

        return hr;
    }

    private static RecruitmentDriveResponseDto MapToResponse(
        RecruitmentDrive drive)
    {
        return new RecruitmentDriveResponseDto
        {
            Id = drive.Id,
            JobOpeningId = drive.JobOpeningId,
            JobTitle = drive.JobOpening.JobTitle,
            CollegeId = drive.CollegeId,
            CollegeName = drive.College.CollegeName,
            DriveName = drive.DriveName,
            DriveDate = drive.DriveDate,
            Venue = drive.Venue,
            Description = drive.Description,
            Status = drive.Status,
            CreatedAt = drive.CreatedAt
        };
    }
}
