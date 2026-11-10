
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CampusPlacementManagement.DTOs.Applications;

namespace CampusPlacementManagement.Tests.Services;

public class JobApplicationServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly FakeJobApplicationRepository _applicationRepository;
    private readonly FakeStudentRepository _studentRepository;
    private readonly FakeCompanyHRRepository _companyHRRepository;
    private readonly FakeJobRepository _jobRepository;
    private readonly JobApplicationService _service;

    private readonly Student _student;
    private readonly JobOpening _job;

    public JobApplicationServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new ApplicationDbContext(options);

        _student = CreateStudent();
        _job = CreateJob();

        _studentRepository = new FakeStudentRepository
        {
            Student = _student
        };

        _jobRepository = new FakeJobRepository
        {
            Job = _job
        };

        _applicationRepository = new FakeJobApplicationRepository
        {
            Student = _student,
            Job = _job
        };

        _companyHRRepository = new FakeCompanyHRRepository();

        _service = new JobApplicationService(
            _applicationRepository,
            _studentRepository,
            _companyHRRepository,
            _jobRepository,
            _context);
    }

    // =====================================================
    // BATCH 1: ApplyAsync() TESTS
    // =====================================================

    [Fact]
    public async Task ApplyAsync_EligibleStudent_ReturnsApplication()
    {
        var result = await _service.ApplyAsync(10, "student-1");

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(10, result.JobOpeningId);
        Assert.Equal("Software Developer", result.JobTitle);
        Assert.Equal("ABC Technologies", result.CompanyName);
        Assert.Equal(JobApplicationStatus.Applied, result.Status);
        Assert.StartsWith("APP-", result.ApplicationReference);

        Assert.Single(_context.ApplicationStatusHistories);
    }

    [Fact]
    public async Task ApplyAsync_StudentNotFound_ThrowsKeyNotFoundException()
    {
        _studentRepository.Student = null;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_InactiveStudent_ThrowsInvalidOperationException()
    {
        _student.User.AccountStatus = AccountStatus.Suspended;

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_UnapprovedCollege_ThrowsInvalidOperationException()
    {
        _student.College.Status = CollegeStatus.Pending;

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_JobNotFound_ThrowsKeyNotFoundException()
    {
        _jobRepository.Job = null;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_UnpublishedJob_ThrowsInvalidOperationException()
    {
        _job.Status = JobStatus.Draft;

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_ExpiredDeadline_ThrowsInvalidOperationException()
    {
        _job.ApplicationDeadline = DateTime.UtcNow.AddDays(-1);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_IneligibleStudent_ThrowsInvalidOperationException()
    {
        _student.Department = "ECE";

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    [Fact]
    public async Task ApplyAsync_DuplicateApplication_ThrowsInvalidOperationException()
    {
        _applicationRepository.ExistingApplication = new JobApplication
        {
            Id = 5,
            StudentId = _student.Id,
            JobOpeningId = _job.Id
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApplyAsync(10, "student-1"));
    }

    // =====================================================
    // BATCH 2: VIEW APPLICATIONS TESTS
    // =====================================================

    [Fact]
    public async Task GetMyApplications_ExistingStudent_ReturnsApplications()
    {
        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        var result = await _service.GetMyApplicationsAsync("student-1");

        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
        Assert.Equal("Software Developer", result.First().JobTitle);
    }

    [Fact]
    public async Task GetMyApplications_StudentNotFound_ThrowsKeyNotFoundException()
    {
        _studentRepository.Student = null;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetMyApplicationsAsync("student-1"));
    }

    [Fact]
    public async Task GetApplicationsByJob_ValidHR_ReturnsApplications()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        var result = await _service.GetApplicationsByJobAsync(
            10, "hr-1");

        Assert.Single(result);
        Assert.Equal("Software Developer", result.First().JobTitle);
    }

    [Fact]
    public async Task GetApplicationsByJob_WrongCompany_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _job.CompanyId = 99;

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetApplicationsByJobAsync(10, "hr-1"));
    }

    [Fact]
    public async Task GetApplicationsByCompany_ValidHR_ReturnsOnlyCompanyApplications()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var ownApplication = CreateApplication(
            1, 1, 10, _student, _job);

        var anotherJob = CreateJob();
        anotherJob.Id = 20;
        anotherJob.CompanyId = 99;
        anotherJob.Company.CompanyName = "Other Company";

        var otherApplication = CreateApplication(
            2, 1, 20, _student, anotherJob);

        _applicationRepository.Applications.Add(ownApplication);
        _applicationRepository.Applications.Add(otherApplication);

        var result = await _service.GetApplicationsByCompanyAsync("hr-1");

        Assert.Single(result);
        Assert.Equal(10, result.First().JobOpeningId);
    }

    [Fact]
    public async Task GetApplicationsByCompany_HRNotFound_ThrowsKeyNotFoundException()
    {
        _companyHRRepository.HR = null;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetApplicationsByCompanyAsync("hr-1"));
    }

    [Fact]
    public async Task GetApplicationsByCompany_InactiveHR_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _companyHRRepository.HR.User.AccountStatus =
            AccountStatus.Suspended;

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetApplicationsByCompanyAsync("hr-1"));
    }

    [Fact]
    public async Task GetApplicationsByCompany_UnapprovedCompany_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _companyHRRepository.HR.Company.Status =
            CompanyStatus.Pending;

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetApplicationsByCompanyAsync("hr-1"));
    }


    // =====================================================
    // BATCH 3: UPDATE APPLICATION STATUS TESTS
    // =====================================================

    [Fact]
    public async Task UpdateStatus_UnderReview_ReturnsUpdatedApplication()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview,
            Remarks = "Application is under review."
        };

        var result = await _service.UpdateStatusAsync(
            1, request, "hr-1");

        Assert.NotNull(result);
        Assert.Equal(JobApplicationStatus.UnderReview, result.Status);
        Assert.NotNull(result.UpdatedAt);
        Assert.Single(_context.ApplicationStatusHistories);
    }

    [Fact]
    public async Task UpdateStatus_Shortlisted_ReturnsUpdatedApplication()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.Shortlisted,
            Remarks = "Student shortlisted."
        };

        var result = await _service.UpdateStatusAsync(
            1, request, "hr-1");

        Assert.NotNull(result);
        Assert.Equal(JobApplicationStatus.Shortlisted, result.Status);
    }

    [Fact]
    public async Task UpdateStatus_Rejected_ReturnsUpdatedApplication()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.Rejected,
            Remarks = "Does not meet requirements."
        };

        var result = await _service.UpdateStatusAsync(
            1, request, "hr-1");

        Assert.NotNull(result);
        Assert.Equal(JobApplicationStatus.Rejected, result.Status);
    }

    [Fact]
    public async Task UpdateStatus_Selected_ReturnsUpdatedApplication()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.Selected,
            Remarks = "Student selected."
        };

        var result = await _service.UpdateStatusAsync(
            1, request, "hr-1");

        Assert.NotNull(result);
        Assert.Equal(JobApplicationStatus.Selected, result.Status);
    }

    [Fact]
    public async Task UpdateStatus_ApplicationNotFound_ReturnsNull()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        var result = await _service.UpdateStatusAsync(
            100, request, "hr-1");

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStatus_HRNotFound_ThrowsKeyNotFoundException()
    {
        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task UpdateStatus_InactiveHR_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _companyHRRepository.HR.User.AccountStatus =
            AccountStatus.Suspended;

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task UpdateStatus_UnapprovedCompany_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _companyHRRepository.HR.Company.Status =
            CompanyStatus.Pending;

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task UpdateStatus_WrongCompany_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var anotherJob = CreateJob();
        anotherJob.CompanyId = 99;

        var application = CreateApplication(
            1, 1, 10, _student, anotherJob);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Theory]
    [InlineData(JobApplicationStatus.Withdrawn)]
    [InlineData(JobApplicationStatus.Rejected)]
    [InlineData(JobApplicationStatus.Selected)]
    public async Task UpdateStatus_TerminalApplication_ThrowsInvalidOperationException(
        JobApplicationStatus currentStatus)
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        application.Status = currentStatus;

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task UpdateStatus_InvalidStatus_ThrowsInvalidOperationException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.Withdrawn
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task UpdateStatus_SameStatus_ThrowsInvalidOperationException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        application.Status = JobApplicationStatus.UnderReview;

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.UnderReview
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateStatusAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task UpdateStatus_ValidStatus_CreatesStatusHistory()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var request = new UpdateJobApplicationStatusRequestDto
        {
            Status = JobApplicationStatus.Shortlisted,
            Remarks = "Good performance."
        };

        await _service.UpdateStatusAsync(1, request, "hr-1");

        var history = await _context.ApplicationStatusHistories
            .FirstOrDefaultAsync();

        Assert.NotNull(history);
        Assert.Equal(1, history.JobApplicationId);
        Assert.Equal(JobApplicationStatus.Shortlisted, history.Status);
        Assert.Equal("Good performance.", history.Remarks);
        Assert.Equal("hr-1", history.ChangedByUserId);
    }


    // =====================================================
    // BATCH 4: WITHDRAW APPLICATION TESTS
    // =====================================================

    [Fact]
    public async Task Withdraw_ValidStudent_ReturnsTrueAndUpdatesStatus()
    {
        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        var result = await _service.WithdrawAsync(1, "student-1");

        Assert.True(result);
        Assert.Equal(JobApplicationStatus.Withdrawn, application.Status);
        Assert.NotNull(application.UpdatedAt);

        var history = await _context.ApplicationStatusHistories
            .ToListAsync();

        Assert.Single(history);
        Assert.Equal(JobApplicationStatus.Withdrawn, history[0].Status);
        Assert.Equal("student-1", history[0].ChangedByUserId);
        Assert.Equal(
            "Application withdrawn by student.",
            history[0].Remarks);
    }

    [Fact]
    public async Task Withdraw_StudentNotFound_ThrowsKeyNotFoundException()
    {
        _studentRepository.Student = null;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.WithdrawAsync(1, "student-1"));
    }

    [Fact]
    public async Task Withdraw_ApplicationNotFound_ReturnsFalse()
    {
        var result = await _service.WithdrawAsync(100, "student-1");

        Assert.False(result);
    }

    [Fact]
    public async Task Withdraw_OtherStudentsApplication_ThrowsUnauthorizedAccessException()
    {
        var anotherStudent = CreateStudent();
        anotherStudent.Id = 2;
        anotherStudent.UserId = "student-2";

        var application = CreateApplication(
            1, 2, 10, anotherStudent, _job);

        _applicationRepository.Applications.Add(application);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.WithdrawAsync(1, "student-1"));
    }

    [Fact]
    public async Task Withdraw_AlreadyWithdrawn_ThrowsInvalidOperationException()
    {
        var application = CreateApplication(
            1, 1, 10, _student, _job);

        application.Status = JobApplicationStatus.Withdrawn;

        _applicationRepository.Applications.Add(application);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.WithdrawAsync(1, "student-1"));
    }

    [Fact]
    public async Task Withdraw_RejectedApplication_ThrowsInvalidOperationException()
    {
        var application = CreateApplication(
            1, 1, 10, _student, _job);

        application.Status = JobApplicationStatus.Rejected;

        _applicationRepository.Applications.Add(application);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.WithdrawAsync(1, "student-1"));
    }

    [Fact]
    public async Task Withdraw_SelectedApplication_ThrowsInvalidOperationException()
    {
        var application = CreateApplication(
            1, 1, 10, _student, _job);

        application.Status = JobApplicationStatus.Selected;

        _applicationRepository.Applications.Add(application);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.WithdrawAsync(1, "student-1"));
    }


    // =====================================================
    // BATCH 5: APPLICATION HISTORY TESTS
    // =====================================================

    [Fact]
    public async Task GetApplicationHistory_StudentOwnApplication_ReturnsHistory()
    {
        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        _context.ApplicationStatusHistories.Add(
            new ApplicationStatusHistory
            {
                JobApplicationId = 1,
                Status = JobApplicationStatus.Applied,
                Remarks = "Application submitted.",
                ChangedByUserId = "student-1",
                ChangedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();

        var result = await _service.GetApplicationHistoryAsync(
            1, "student-1", "Student");

        var history = result.ToList();

        Assert.Single(history);
        Assert.Equal(JobApplicationStatus.Applied, history[0].Status);
        Assert.Equal("student-1", history[0].ChangedByUserId);
    }

    [Fact]
    public async Task GetApplicationHistory_OtherStudent_ThrowsUnauthorizedAccessException()
    {
        var application = CreateApplication(
            1, 1, 10, _student, _job);

        _applicationRepository.Applications.Add(application);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetApplicationHistoryAsync(
                1, "another-student", "Student"));
    }

    [Fact]
    public async Task GetApplicationHistory_ValidHR_ReturnsHistory()
    {
        _companyHRRepository.HR = CreateActiveHR();

        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        _context.ApplicationStatusHistories.Add(
            new ApplicationStatusHistory
            {
                JobApplicationId = 1,
                Status = JobApplicationStatus.UnderReview,
                Remarks = "Under review.",
                ChangedByUserId = "hr-1",
                ChangedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();

        var result = await _service.GetApplicationHistoryAsync(
            1, "hr-1", "CompanyHR");

        Assert.Single(result);
        Assert.Equal(
            JobApplicationStatus.UnderReview,
            result.First().Status);
    }

    [Fact]
    public async Task GetApplicationHistory_WrongCompanyHR_ThrowsUnauthorizedAccessException()
    {
        _companyHRRepository.HR = CreateActiveHR();

        var anotherJob = CreateJob();
        anotherJob.CompanyId = 99;

        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, anotherJob));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetApplicationHistoryAsync(
                1, "hr-1", "CompanyHR"));
    }

    [Fact]
    public async Task GetApplicationHistory_SuperAdmin_ReturnsHistory()
    {
        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        _context.ApplicationStatusHistories.Add(
            new ApplicationStatusHistory
            {
                JobApplicationId = 1,
                Status = JobApplicationStatus.Applied,
                Remarks = "Submitted.",
                ChangedByUserId = "student-1",
                ChangedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();

        var result = await _service.GetApplicationHistoryAsync(
            1, "admin-1", "SuperAdmin");

        Assert.Single(result);
    }

    [Fact]
    public async Task GetApplicationHistory_InvalidRole_ThrowsUnauthorizedAccessException()
    {
        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetApplicationHistoryAsync(
                1, "user-1", "PlacementOfficer"));
    }

    [Fact]
    public async Task GetApplicationHistory_ApplicationNotFound_ThrowsKeyNotFoundException()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetApplicationHistoryAsync(
                100, "student-1", "Student"));
    }

    [Fact]
    public async Task GetApplicationHistory_HRNotFound_ThrowsKeyNotFoundException()
    {
        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        _companyHRRepository.HR = null;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetApplicationHistoryAsync(
                1, "hr-1", "CompanyHR"));
    }

    // =====================================================
    // BATCH 5: AUTHORIZATION HELPER TESTS
    // =====================================================

    [Fact]
    public async Task GetCompanyIdByJobId_ExistingJob_ReturnsCompanyId()
    {
        var result = await _service.GetCompanyIdByJobIdAsync(10);

        Assert.Equal(20, result);
    }

    [Fact]
    public async Task GetCompanyIdByJobId_MissingJob_ReturnsNull()
    {
        _jobRepository.Job = null;

        var result = await _service.GetCompanyIdByJobIdAsync(10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCompanyIdByApplicationId_ExistingApplication_ReturnsCompanyId()
    {
        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        var result =
            await _service.GetCompanyIdByApplicationIdAsync(1);

        Assert.Equal(20, result);
    }

    [Fact]
    public async Task GetCompanyIdByApplicationId_MissingApplication_ReturnsNull()
    {
        var result =
            await _service.GetCompanyIdByApplicationIdAsync(100);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetApplicationForAuthorization_ExistingApplication_ReturnsCompanyId()
    {
        _applicationRepository.Applications.Add(
            CreateApplication(1, 1, 10, _student, _job));

        var result =
            await _service.GetApplicationForAuthorizationAsync(1);

        Assert.NotNull(result);
        Assert.Equal(20, result.CompanyId);
    }

    [Fact]
    public async Task GetApplicationForAuthorization_MissingApplication_ReturnsNull()
    {
        var result =
            await _service.GetApplicationForAuthorizationAsync(100);

        Assert.Null(result);
    }




    // =====================================================
    // TEST DATA HELPERS
    // =====================================================

    private static Student CreateStudent()
    {
        return new Student
        {
            Id = 1,
            UserId = "student-1",
            CollegeId = 1,
            Department = "CSE",
            GraduationYear = 2027,
            RegisterNumber = "REG001",

            User = new ApplicationUser
            {
                Id = "student-1",
                UserName = "Test Student",
                Email = "student@test.com",
                AccountStatus = AccountStatus.Active
            },

            College = new College
            {
                Id = 1,
                Status = CollegeStatus.Approved
            }
        };
    }

    private static JobOpening CreateJob()
    {
        return new JobOpening
        {
            Id = 10,
            CompanyId = 20,
            JobTitle = "Software Developer",
            Status = JobStatus.Published,
            ApplicationDeadline = DateTime.UtcNow.AddDays(10),

            Company = new Company
            {
                Id = 20,
                CompanyName = "ABC Technologies"
            },

            TargetColleges = new List<JobTargetCollege>
            {
                new() { CollegeId = 1 }
            },

            EligibleDepartments = new List<JobEligibleDepartment>
            {
                new() { DepartmentName = "CSE" }
            },

            EligibleGraduationYears = new List<JobEligibleGraduationYear>
            {
                new() { GraduationYear = 2027 }
            }
        };
    }

    private static CompanyHR CreateActiveHR()
    {
        return new CompanyHR
        {
            Id = 1,
            UserId = "hr-1",
            CompanyId = 20,

            User = new ApplicationUser
            {
                Id = "hr-1",
                UserName = "Test HR",
                Email = "hr@test.com",
                AccountStatus = AccountStatus.Active
            },

            Company = new Company
            {
                Id = 20,
                CompanyName = "ABC Technologies",
                Status = CompanyStatus.Approved
            }
        };
    }

    private static JobApplication CreateApplication(
        int id,
        int studentId,
        int jobId,
        Student student,
        JobOpening job)
    {
        return new JobApplication
        {
            Id = id,
            StudentId = studentId,
            JobOpeningId = jobId,
            ApplicationReference = $"APP-{id}",
            Status = JobApplicationStatus.Applied,
            AppliedAt = DateTime.UtcNow,
            Student = student,
            JobOpening = job
        };
    }

    // =====================================================
    // FAKE STUDENT REPOSITORY
    // =====================================================

    private class FakeStudentRepository : IStudentRepository
    {
        public Student? Student { get; set; }

        public Task<Student?> GetByUserIdAsync(string userId)
            => Task.FromResult(Student);

        public Task<IEnumerable<Student>> GetAllAsync()
            => Task.FromResult<IEnumerable<Student>>(
                Student == null
                    ? new List<Student>()
                    : new List<Student> { Student });

        public Task<Student?> GetByIdAsync(int id)
            => Task.FromResult(Student);

        public Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId)
            => Task.FromResult<IEnumerable<Student>>(new List<Student>());

        public Task<IEnumerable<Student>> GetPendingAsync(int collegeId)
            => Task.FromResult<IEnumerable<Student>>(new List<Student>());

        public Task AddAsync(Student student)
            => Task.CompletedTask;

        public Task UpdateAsync(Student student)
            => Task.CompletedTask;

        public Task<bool> ExistsAsync(int id)
            => Task.FromResult(false);

        public Task<bool> RegisterNumberExistsAsync(string registerNumber)
            => Task.FromResult(false);
    }

    // =====================================================
    // FAKE JOB REPOSITORY
    // =====================================================

    private class FakeJobRepository : IJobRepository
    {
        public JobOpening? Job { get; set; }

        public Task<JobOpening?> GetByIdAsync(int id)
            => Task.FromResult(Job);

        public Task<IEnumerable<JobOpening>> GetAllAsync()
            => Task.FromResult<IEnumerable<JobOpening>>(
                Job == null
                    ? new List<JobOpening>()
                    : new List<JobOpening> { Job });

        public Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId)
            => Task.FromResult<IEnumerable<JobOpening>>(new List<JobOpening>());

        public Task<IEnumerable<JobOpening>> GetPublishedAsync()
            => Task.FromResult<IEnumerable<JobOpening>>(new List<JobOpening>());

        public Task AddAsync(JobOpening job)
            => Task.CompletedTask;

        public Task UpdateAsync(JobOpening job)
            => Task.CompletedTask;

        public Task<bool> ExistsAsync(int id)
            => Task.FromResult(false);
    }

    // =====================================================
    // FAKE COMPANY HR REPOSITORY
    // =====================================================

    private class FakeCompanyHRRepository : ICompanyHRRepository
    {
        public CompanyHR? HR { get; set; }

        public Task<CompanyHR?> GetByUserIdAsync(string userId)
            => Task.FromResult(HR);

        public Task<IEnumerable<CompanyHR>> GetAllAsync()
            => Task.FromResult<IEnumerable<CompanyHR>>(
                new List<CompanyHR>());

        public Task<CompanyHR?> GetByIdAsync(int id)
            => Task.FromResult<CompanyHR?>(null);

        public Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId)
            => Task.FromResult<IEnumerable<CompanyHR>>(
                new List<CompanyHR>());

        public Task AddAsync(CompanyHR companyHR)
            => Task.CompletedTask;

        public Task UpdateAsync(CompanyHR companyHR)
            => Task.CompletedTask;

        public Task<bool> ExistsAsync(int id)
            => Task.FromResult(false);
    }

    // =====================================================
    // FAKE JOB APPLICATION REPOSITORY
    // =====================================================

    private class FakeJobApplicationRepository : IJobApplicationRepository
    {
        public Student Student { get; set; } = null!;
        public JobOpening Job { get; set; } = null!;

        public JobApplication? ExistingApplication { get; set; }
        public JobApplication? SavedApplication { get; private set; }

        public List<JobApplication> Applications { get; set; } = new();

        public Task<JobApplication?> GetByStudentAndJobAsync(
            int studentId,
            int jobOpeningId)
            => Task.FromResult(ExistingApplication);

        public Task AddAsync(JobApplication application)
        {
            application.Id = 1;
            application.Student = Student;
            application.JobOpening = Job;

            SavedApplication = application;

            return Task.CompletedTask;
        }

        public Task<JobApplication?> GetByIdAsync(int id)
        {
            var application = Applications.FirstOrDefault(a => a.Id == id);

            if (application == null && SavedApplication?.Id == id)
            {
                application = SavedApplication;
            }

            return Task.FromResult(application);
        }

        public Task<IEnumerable<JobApplication>> GetAllAsync()
            => Task.FromResult<IEnumerable<JobApplication>>(Applications);

        public Task<IEnumerable<JobApplication>> GetByStudentIdAsync(int studentId)
            => Task.FromResult<IEnumerable<JobApplication>>(
                Applications.Where(a => a.StudentId == studentId));

        public Task<IEnumerable<JobApplication>> GetByJobIdAsync(int jobOpeningId)
            => Task.FromResult<IEnumerable<JobApplication>>(
                Applications.Where(a => a.JobOpeningId == jobOpeningId));

        public Task UpdateAsync(JobApplication application)
        {
            SavedApplication = application;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
            => Task.FromResult(
                Applications.Any(a => a.Id == id) ||
                SavedApplication?.Id == id);
    }
}
