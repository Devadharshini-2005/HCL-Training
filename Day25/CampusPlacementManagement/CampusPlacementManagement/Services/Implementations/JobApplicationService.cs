using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Applications;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICompanyHRRepository _companyHRRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ApplicationDbContext _context;

    public JobApplicationService(
        IJobApplicationRepository applicationRepository,
        IStudentRepository studentRepository,
        ICompanyHRRepository companyHRRepository,
        IJobRepository jobRepository,
        ApplicationDbContext context)
    {
        _applicationRepository = applicationRepository;
        _studentRepository = studentRepository;
        _companyHRRepository = companyHRRepository;
        _jobRepository = jobRepository;
        _context = context;
    }

    // Student applies for a job
    public async Task<ApplyJobResponseDto> ApplyAsync(
        int jobOpeningId,
        string studentUserId)
    {
        var student = await _studentRepository
            .GetByUserIdAsync(studentUserId);

        if (student == null)
            throw new KeyNotFoundException("Student profile not found.");

        if (student.User.AccountStatus != AccountStatus.Active)
            throw new InvalidOperationException("Student account is not active.");

        if (student.College.Status != CollegeStatus.Approved)
            throw new InvalidOperationException("Your college is not approved.");

        var job = await _jobRepository.GetByIdAsync(jobOpeningId);

        if (job == null)
            throw new KeyNotFoundException("Job not found.");

        if (job.Status != JobStatus.Published)
            throw new InvalidOperationException("This job is not available.");

        if (job.ApplicationDeadline <= DateTime.UtcNow)
            throw new InvalidOperationException("Application deadline has passed.");

        bool collegeEligible = job.TargetColleges
            .Any(c => c.CollegeId == student.CollegeId);

        bool departmentEligible = job.EligibleDepartments
            .Any(d => string.Equals(
                d.DepartmentName,
                student.Department,
                StringComparison.OrdinalIgnoreCase));

        bool graduationYearEligible = job.EligibleGraduationYears
            .Any(y => y.GraduationYear == student.GraduationYear);

        if (!collegeEligible || !departmentEligible || !graduationYearEligible)
        {
            throw new InvalidOperationException(
                "You are not eligible for this job.");
        }

        var existingApplication =
            await _applicationRepository.GetByStudentAndJobAsync(
                student.Id,
                jobOpeningId);

        if (existingApplication != null)
        {
            throw new InvalidOperationException(
                "You have already applied for this job.");
        }

        var application = new JobApplication
        {
            StudentId = student.Id,
            JobOpeningId = jobOpeningId,
            ApplicationReference = $"APP-{Guid.NewGuid():N}",
            Status = JobApplicationStatus.Applied,
            AppliedAt = DateTime.UtcNow
        };

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        await _applicationRepository.AddAsync(application);

        _context.ApplicationStatusHistories.Add(
            new ApplicationStatusHistory
            {
                JobApplicationId = application.Id,
                Status = JobApplicationStatus.Applied,
                Remarks = "Application submitted.",
                ChangedByUserId = studentUserId,
                ChangedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var savedApplication =
            await _applicationRepository.GetByIdAsync(application.Id);

        return MapToResponse(savedApplication!);
    }

    // Student views their own applications
    public async Task<IEnumerable<ApplyJobResponseDto>>
        GetMyApplicationsAsync(string studentUserId)
    {
        var student = await _studentRepository
            .GetByUserIdAsync(studentUserId);

        if (student == null)
            throw new KeyNotFoundException("Student profile not found.");

        var applications =
            await _applicationRepository.GetByStudentIdAsync(student.Id);

        return applications.Select(MapToResponse);
    }

    // Company HR views applications for one job
    public async Task<IEnumerable<ApplyJobResponseDto>>
        GetApplicationsByJobAsync(
            int jobOpeningId,
            string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var job = await _jobRepository.GetByIdAsync(jobOpeningId);

        if (job == null)
            throw new KeyNotFoundException("Job not found.");

        EnsureJobOwnership(job, hr.CompanyId);

        var applications =
            await _applicationRepository.GetByJobIdAsync(jobOpeningId);

        return applications.Select(MapToResponse);
    }

    // Company HR views all applications for their company
    public async Task<IEnumerable<ApplyJobResponseDto>>
        GetApplicationsByCompanyAsync(string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var applications =
            await _applicationRepository.GetAllAsync();

        return applications
            .Where(a => a.JobOpening.CompanyId == hr.CompanyId)
            .Select(MapToResponse);
    }

    // Company HR updates application status
    public async Task<ApplyJobResponseDto?> UpdateStatusAsync(
        int applicationId,
        UpdateJobApplicationStatusRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var application =
            await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
            return null;

        EnsureJobOwnership(application.JobOpening, hr.CompanyId);

        if (application.Status == JobApplicationStatus.Withdrawn ||
            application.Status == JobApplicationStatus.Rejected ||
            application.Status == JobApplicationStatus.Selected)
        {
            throw new InvalidOperationException(
                "This application can no longer be updated.");
        }

        if (request.Status != JobApplicationStatus.UnderReview &&
            request.Status != JobApplicationStatus.Shortlisted &&
            request.Status != JobApplicationStatus.Rejected &&
            request.Status != JobApplicationStatus.Selected)
        {
            throw new InvalidOperationException(
                "Invalid status for Company HR.");
        }

        if (application.Status == request.Status)
        {
            throw new InvalidOperationException(
                "Application already has this status.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        application.Status = request.Status;
        application.UpdatedAt = DateTime.UtcNow;

        await _applicationRepository.UpdateAsync(application);

        _context.ApplicationStatusHistories.Add(
            new ApplicationStatusHistory
            {
                JobApplicationId = applicationId,
                Status = request.Status,
                Remarks = request.Remarks,
                ChangedByUserId = companyHRUserId,
                ChangedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return MapToResponse(application);
    }

    // Student withdraws their own application
    public async Task<bool> WithdrawAsync(
        int applicationId,
        string studentUserId)
    {
        var student = await _studentRepository
            .GetByUserIdAsync(studentUserId);

        if (student == null)
            throw new KeyNotFoundException("Student profile not found.");

        var application =
            await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
            return false;

        if (application.StudentId != student.Id)
        {
            throw new UnauthorizedAccessException(
                "You cannot withdraw another student's application.");
        }

        if (application.Status == JobApplicationStatus.Withdrawn ||
            application.Status == JobApplicationStatus.Rejected ||
            application.Status == JobApplicationStatus.Selected)
        {
            throw new InvalidOperationException(
                "This application cannot be withdrawn.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        application.Status = JobApplicationStatus.Withdrawn;
        application.UpdatedAt = DateTime.UtcNow;

        await _applicationRepository.UpdateAsync(application);

        _context.ApplicationStatusHistories.Add(
            new ApplicationStatusHistory
            {
                JobApplicationId = applicationId,
                Status = JobApplicationStatus.Withdrawn,
                Remarks = "Application withdrawn by student.",
                ChangedByUserId = studentUserId,
                ChangedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }

    // View application status history
    public async Task<IEnumerable<ApplicationStatusHistoryResponseDto>>
        GetApplicationHistoryAsync(
            int applicationId,
            string userId,
            string userRole)
    {
        var application =
            await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
            throw new KeyNotFoundException("Application not found.");

        if (userRole == "Student")
        {
            if (application.Student.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You cannot view another student's application history.");
        }
        else if (userRole == "CompanyHR")
        {
            var hr = await GetActiveCompanyHRAsync(userId);

            EnsureJobOwnership(application.JobOpening, hr.CompanyId);
        }
        else if (userRole != "SuperAdmin")
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to view application history.");
        }

        var history = await _context.ApplicationStatusHistories
            .Where(h => h.JobApplicationId == applicationId)
            .OrderBy(h => h.ChangedAt)
            .AsNoTracking()
            .ToListAsync();

        return history.Select(h => new ApplicationStatusHistoryResponseDto
        {
            Id = h.Id,
            Status = h.Status,
            Remarks = h.Remarks,
            ChangedByUserId = h.ChangedByUserId,
            ChangedAt = h.ChangedAt
        });
    }

    // Verify active Company HR and approved company
    private async Task<CompanyHR> GetActiveCompanyHRAsync(string userId)
    {
        var hr = await _companyHRRepository.GetByUserIdAsync(userId);

        if (hr == null)
            throw new KeyNotFoundException("Company HR profile not found.");

        if (hr.User.AccountStatus != AccountStatus.Active)
            throw new UnauthorizedAccessException(
                "Company HR account is not active.");

        if (hr.Company.Status != CompanyStatus.Approved)
            throw new UnauthorizedAccessException(
                "Company is not approved.");

        return hr;
    }

    // Verify job belongs to the HR's company
    private static void EnsureJobOwnership(
        JobOpening job,
        int companyId)
    {
        if (job.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot access another company's job.");
        }
    }

    // Convert entity into response DTO
    private static ApplyJobResponseDto MapToResponse(
        JobApplication application)
    {
        return new ApplyJobResponseDto
        {
            Id = application.Id,
            ApplicationReference = application.ApplicationReference,
            JobOpeningId = application.JobOpeningId,
            JobTitle = application.JobOpening.JobTitle,
            CompanyName = application.JobOpening.Company.CompanyName,
            StudentName = application.Student.User.UserName ?? string.Empty,
            StudentEmail = application.Student.User.Email ?? string.Empty,
            RegisterNumber = application.Student.RegisterNumber,
            Department = application.Student.Department,
            Status = application.Status,
            AppliedAt = application.AppliedAt,
            UpdatedAt = application.UpdatedAt
        };
    }
}