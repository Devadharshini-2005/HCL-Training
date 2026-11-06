
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Reports;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Reports;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PlatformOverviewReportDto> GetPlatformOverviewAsync()
    {
        return new PlatformOverviewReportDto
        {
            TotalColleges = await _context.Colleges.CountAsync(),
            ApprovedColleges = await _context.Colleges.CountAsync(
                c => c.Status == CollegeStatus.Approved),

            TotalCompanies = await _context.Companies.CountAsync(),
            ApprovedCompanies = await _context.Companies.CountAsync(
                c => c.Status == CompanyStatus.Approved),

            TotalStudents = await _context.Students.CountAsync(),
            TotalPlacementOfficers = await _context.PlacementOfficers.CountAsync(),
            TotalCompanyHRs = await _context.CompanyHRs.CountAsync(),

            TotalJobs = await _context.JobOpenings.CountAsync(),
            PublishedJobs = await _context.JobOpenings.CountAsync(
                j => j.Status == JobStatus.Published),

            TotalApplications = await _context.JobApplications.CountAsync(),
            SelectedStudents = await _context.JobApplications.CountAsync(
                a => a.Status == JobApplicationStatus.Selected)
        };
    }

    public async Task<CollegeOverviewReportDto> GetCollegeOverviewAsync(int collegeId)
    {
        var college = await _context.Colleges
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == collegeId);

        if (college == null)
            throw new KeyNotFoundException("College not found.");

        var students = _context.Students
            .Where(s => s.CollegeId == collegeId);

        var studentIds = students.Select(s => s.Id);

        return new CollegeOverviewReportDto
        {
            CollegeId = college.Id,
            CollegeName = college.CollegeName,

            TotalStudents = await students.CountAsync(),

            ApprovedStudents = await students.CountAsync(
                s => s.User.AccountStatus == AccountStatus.Active),

            PendingStudents = await students.CountAsync(
                s => s.User.AccountStatus == AccountStatus.Pending),

            TotalApplications = await _context.JobApplications.CountAsync(
                a => studentIds.Contains(a.StudentId)),

            SelectedStudents = await _context.JobApplications
                .Where(a => studentIds.Contains(a.StudentId)
                    && a.Status == JobApplicationStatus.Selected)
                .Select(a => a.StudentId)
                .Distinct()
                .CountAsync(),

            TotalPlacementOfficers = await _context.PlacementOfficers.CountAsync(
                p => p.CollegeId == collegeId),

            TotalJobsAvailable = await _context.JobTargetColleges.CountAsync(
                j => j.CollegeId == collegeId
                    && j.JobOpening.Status == JobStatus.Published)
        };
    }

    public async Task<CompanyRecruitmentReportDto> GetCompanyRecruitmentAsync(int companyId)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null)
            throw new KeyNotFoundException("Company not found.");

        var jobs = _context.JobOpenings
            .Where(j => j.CompanyId == companyId);

        var jobIds = jobs.Select(j => j.Id);

        return new CompanyRecruitmentReportDto
        {
            CompanyId = company.Id,
            CompanyName = company.CompanyName,

            TotalJobsPosted = await jobs.CountAsync(),

            PublishedJobs = await jobs.CountAsync(
                j => j.Status == JobStatus.Published),

            ClosedJobs = await jobs.CountAsync(
                j => j.Status == JobStatus.Closed),

            TotalApplicationsReceived = await _context.JobApplications.CountAsync(
                a => jobIds.Contains(a.JobOpeningId)),

            TotalStudentsSelected = await _context.JobApplications
                .Where(a => jobIds.Contains(a.JobOpeningId)
                    && a.Status == JobApplicationStatus.Selected)
                .Select(a => a.StudentId)
                .Distinct()
                .CountAsync(),

            TotalRecruitmentDrives = await _context.RecruitmentDrives.CountAsync(
                d => jobIds.Contains(d.JobOpeningId)),

            TotalInterviewsScheduled = await _context.InterviewSchedules.CountAsync(
                i => jobIds.Contains(i.JobApplication.JobOpeningId))
        };
    }

    public async Task<List<StudentPlacementReportDto>> GetStudentPlacementAsync(int collegeId)
    {
        var students = await _context.Students
            .AsNoTracking()
            .Where(s => s.CollegeId == collegeId)
            .Select(s => new StudentPlacementReportDto
            {
                StudentId = s.Id,
                StudentName = s.User.UserName ?? string.Empty,
                RegisterNumber = s.RegisterNumber,
                Department = s.Department,
                GraduationYear = s.GraduationYear,

                TotalApplications = s.JobApplications.Count(),

                ShortlistedApplications = s.JobApplications.Count(
                    a => a.Status == JobApplicationStatus.Shortlisted),

                SelectedApplications = s.JobApplications.Count(
                    a => a.Status == JobApplicationStatus.Selected),

                IsPlaced = s.JobApplications.Any(
                    a => a.Status == JobApplicationStatus.Selected)
            })
            .ToListAsync();

        return students;
    }
}
