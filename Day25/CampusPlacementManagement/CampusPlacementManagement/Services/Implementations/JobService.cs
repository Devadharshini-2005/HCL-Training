
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Jobs;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly ICompanyHRRepository _companyHRRepository;
    private readonly ApplicationDbContext _context;

    private readonly IStudentRepository _studentRepository;

    public JobService(
    IJobRepository jobRepository,
    ICompanyHRRepository companyHRRepository,
    IStudentRepository studentRepository,
    ApplicationDbContext context)
    {
        _jobRepository = jobRepository;
        _companyHRRepository = companyHRRepository;
        _studentRepository = studentRepository;
        _context = context;
    }

    public async Task<IEnumerable<JobResponseDto>> GetAllAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();

        return jobs.Select(MapToResponse);
    }

    public async Task<JobResponseDto?> GetByIdAsync(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        return job == null ? null : MapToResponse(job);
    }

    public async Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(
        int companyId)
    {
        var jobs = await _jobRepository.GetByCompanyIdAsync(companyId);

        return jobs.Select(MapToResponse);
    }


    public async Task<IEnumerable<JobResponseDto>> GetPublishedAsync(
        string studentUserId)
    {
        var student = await _studentRepository.GetByUserIdAsync(studentUserId);

        if (student == null)
        {
            throw new UnauthorizedAccessException(
                "Student profile was not found.");
        }

        if (student.User.AccountStatus != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException(
                "Your student account is not active.");
        }

        if (student.College.Status != CollegeStatus.Approved)
        {
            throw new UnauthorizedAccessException(
                "Your college is not approved.");
        }

        var jobs = await _jobRepository.GetPublishedAsync();

        var eligibleJobs = jobs.Where(job =>
            job.ApplicationDeadline > DateTime.UtcNow &&
            job.TargetColleges.Any(tc =>
                tc.CollegeId == student.CollegeId) &&
            job.EligibleDepartments.Any(d =>
                d.DepartmentName.Equals(
                    student.Department,
                    StringComparison.OrdinalIgnoreCase)) &&
            job.EligibleGraduationYears.Any(y =>
                y.GraduationYear == student.GraduationYear));

        return eligibleJobs.Select(MapToResponse);
    }


    public async Task<JobResponseDto> CreateAsync(
        CreateJobRequestDto request,
        string companyHRUserId)
    {
        var companyHR = await GetActiveCompanyHRAsync(companyHRUserId);

        await ValidateTargetCollegesAsync(request.TargetCollegeIds);

        if (request.ApplicationDeadline <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Application deadline must be in the future.");
        }

        var job = new JobOpening
        {
            CompanyId = companyHR.CompanyId,
            JobTitle = request.JobTitle,
            JobDescription = request.JobDescription,
            RequiredSkills = request.RequiredSkills,
            JobLocation = request.JobLocation,
            Salary = request.Salary,
            ApplicationDeadline = request.ApplicationDeadline,
            Status = JobStatus.Draft,
            CreatedAt = DateTime.UtcNow,

            TargetColleges = request.TargetCollegeIds
                .Distinct()
                .Select(id => new JobTargetCollege
                {
                    CollegeId = id
                }).ToList(),

            EligibleDepartments = request.EligibleDepartments
                .Distinct()
                .Select(department => new JobEligibleDepartment
                {
                    DepartmentName = department.Trim()
                }).ToList(),

            EligibleGraduationYears = request.EligibleGraduationYears
                .Distinct()
                .Select(year => new JobEligibleGraduationYear
                {
                    GraduationYear = year
                }).ToList()
        };

        await _jobRepository.AddAsync(job);

        var savedJob = await _jobRepository.GetByIdAsync(job.Id);

        return MapToResponse(savedJob!);
    }

    public async Task<JobResponseDto?> UpdateAsync(
        int id,
        UpdateJobRequestDto request,
        string companyHRUserId)
    {
        var companyHR = await GetActiveCompanyHRAsync(companyHRUserId);

        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            return null;

        EnsureJobOwnership(job, companyHR.CompanyId);

        if (job.Status != JobStatus.Draft)
        {
            throw new InvalidOperationException(
                "Only draft jobs can be updated.");
        }

        await ValidateTargetCollegesAsync(request.TargetCollegeIds);

        if (request.ApplicationDeadline <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Application deadline must be in the future.");
        }

        job.JobTitle = request.JobTitle;
        job.JobDescription = request.JobDescription;
        job.RequiredSkills = request.RequiredSkills;
        job.JobLocation = request.JobLocation;
        job.Salary = request.Salary;
        job.ApplicationDeadline = request.ApplicationDeadline;
        job.UpdatedAt = DateTime.UtcNow;

        job.TargetColleges.Clear();
        foreach (var collegeId in request.TargetCollegeIds.Distinct())
        {
            job.TargetColleges.Add(new JobTargetCollege
            {
                CollegeId = collegeId
            });
        }

        job.EligibleDepartments.Clear();
        foreach (var department in request.EligibleDepartments.Distinct())
        {
            job.EligibleDepartments.Add(new JobEligibleDepartment
            {
                DepartmentName = department.Trim()
            });
        }

        job.EligibleGraduationYears.Clear();
        foreach (var year in request.EligibleGraduationYears.Distinct())
        {
            job.EligibleGraduationYears.Add(new JobEligibleGraduationYear
            {
                GraduationYear = year
            });
        }

        await _jobRepository.UpdateAsync(job);

        return MapToResponse(job);
    }

    public async Task<bool> PublishAsync(
        int id,
        string companyHRUserId)
    {
        var companyHR = await GetActiveCompanyHRAsync(companyHRUserId);

        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            return false;

        EnsureJobOwnership(job, companyHR.CompanyId);

        if (job.Status != JobStatus.Draft)
        {
            throw new InvalidOperationException(
                "Only draft jobs can be published.");
        }

        if (job.ApplicationDeadline <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Cannot publish a job with an expired application deadline.");
        }

        job.Status = JobStatus.Published;
        job.UpdatedAt = DateTime.UtcNow;

        await _jobRepository.UpdateAsync(job);

        return true;
    }

    public async Task<bool> CloseAsync(
        int id,
        string companyHRUserId)
    {
        var companyHR = await GetActiveCompanyHRAsync(companyHRUserId);

        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null)
            return false;

        EnsureJobOwnership(job, companyHR.CompanyId);

        if (job.Status != JobStatus.Published)
        {
            throw new InvalidOperationException(
                "Only published jobs can be closed.");
        }

        job.Status = JobStatus.Closed;
        job.UpdatedAt = DateTime.UtcNow;

        await _jobRepository.UpdateAsync(job);

        return true;
    }

    private async Task<CompanyHR> GetActiveCompanyHRAsync(string userId)
    {
        var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

        if (companyHR == null)
        {
            throw new UnauthorizedAccessException(
                "Company HR profile was not found.");
        }

        if (companyHR.User.AccountStatus != AccountStatus.Active)
        {
            throw new UnauthorizedAccessException(
                "Your Company HR account is not active.");
        }

        if (companyHR.Company.Status != CompanyStatus.Approved)
        {
            throw new UnauthorizedAccessException(
                "Your company is not approved.");
        }

        return companyHR;
    }

    private async Task ValidateTargetCollegesAsync(
        List<int> collegeIds)
    {
        var uniqueIds = collegeIds.Distinct().ToList();

        var approvedCollegeIds = await _context.Colleges
            .Where(c =>
                uniqueIds.Contains(c.Id) &&
                c.Status == CollegeStatus.Approved)
            .Select(c => c.Id)
            .ToListAsync();

        if (approvedCollegeIds.Count != uniqueIds.Count)
        {
            throw new InvalidOperationException(
                "One or more target colleges do not exist or are not approved.");
        }
    }

    private static void EnsureJobOwnership(
        JobOpening job,
        int companyId)
    {
        if (job.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot manage another company's job.");
        }
    }

    private static JobResponseDto MapToResponse(JobOpening job)
    {
        return new JobResponseDto
        {
            Id = job.Id,
            CompanyId = job.CompanyId,
            CompanyName = job.Company.CompanyName,
            JobTitle = job.JobTitle,
            JobDescription = job.JobDescription,
            RequiredSkills = job.RequiredSkills,
            JobLocation = job.JobLocation,
            Salary = job.Salary,
            ApplicationDeadline = job.ApplicationDeadline,
            Status = job.Status,

            TargetCollegeIds = job.TargetColleges
                .Select(tc => tc.CollegeId)
                .ToList(),

            EligibleDepartments = job.EligibleDepartments
                .Select(d => d.DepartmentName)
                .ToList(),

            EligibleGraduationYears = job.EligibleGraduationYears
                .Select(y => y.GraduationYear)
                .ToList(),

            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt
        };
    }


    public async Task<IEnumerable<JobResponseDto>> GetByCompanyUserIdAsync(
        string companyHRUserId)
    {
        var companyHR = await GetActiveCompanyHRAsync(companyHRUserId);

        var jobs = await _jobRepository.GetByCompanyIdAsync(
            companyHR.CompanyId);

        return jobs.Select(MapToResponse);
    }

}
