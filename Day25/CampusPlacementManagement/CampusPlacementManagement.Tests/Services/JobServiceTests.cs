
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Jobs;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Tests.Services;

public class JobServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly FakeJobRepository _jobRepository;
    private readonly FakeCompanyHRRepository _companyHRRepository;
    private readonly FakeStudentRepository _studentRepository;
    private readonly JobService _service;

    public JobServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _jobRepository = new FakeJobRepository();
        _companyHRRepository = new FakeCompanyHRRepository();
        _studentRepository = new FakeStudentRepository();

        _service = new JobService(
            _jobRepository,
            _companyHRRepository,
            _studentRepository,
            _context);
    }

    // ------------------------------------
    // GetAllAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllJobs()
    {
        // Arrange
        _jobRepository.Jobs.Add(CreateJob(1));
        _jobRepository.Jobs.Add(CreateJob(2));

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoJobsExist()
    {
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    // ------------------------------------
    // GetByIdAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetByIdAsync_ShouldReturnJob_WhenJobExists()
    {
        // Arrange
        var job = CreateJob(1);
        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Software Developer", result.JobTitle);
        Assert.Equal("ABC Technologies", result.CompanyName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenJobDoesNotExist()
    {
        // Act
        var result = await _service.GetByIdAsync(100);

        // Assert
        Assert.Null(result);
    }

    // ------------------------------------
    // GetByCompanyIdAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetByCompanyIdAsync_ShouldReturnCompanyJobs()
    {
        // Arrange
        _jobRepository.Jobs.Add(CreateJob(1, companyId: 10));
        _jobRepository.Jobs.Add(CreateJob(2, companyId: 10));
        _jobRepository.Jobs.Add(CreateJob(3, companyId: 20));

        // Act
        var result = await _service.GetByCompanyIdAsync(10);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, job => Assert.Equal(10, job.CompanyId));
    }

    // ------------------------------------
    // GetPublishedAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetPublishedAsync_ShouldReturnEligibleJobs()
    {
        // Arrange
        var student = CreateStudent();
        _studentRepository.Student = student;

        var job = CreateJob(1);
        job.Status = JobStatus.Published;
        job.ApplicationDeadline = DateTime.UtcNow.AddDays(10);

        job.TargetColleges.Add(new JobTargetCollege
        {
            CollegeId = student.CollegeId
        });

        job.EligibleDepartments.Add(new JobEligibleDepartment
        {
            DepartmentName = "CSE"
        });

        job.EligibleGraduationYears.Add(new JobEligibleGraduationYear
        {
            GraduationYear = 2027
        });

        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.GetPublishedAsync("student-user");

        // Assert
        var returnedJobs = result.ToList();

        Assert.Single(returnedJobs);
        Assert.Equal(1, returnedJobs[0].Id);
    }

    [Fact]
    public async Task GetPublishedAsync_ShouldThrowException_WhenStudentDoesNotExist()
    {
        // Arrange
        _studentRepository.Student = null;

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetPublishedAsync("unknown-user"));

        // Assert
        Assert.Equal("Student profile was not found.", exception.Message);
    }

    [Fact]
    public async Task GetPublishedAsync_ShouldThrowException_WhenStudentAccountIsInactive()
    {
        // Arrange
        var student = CreateStudent();
        student.User.AccountStatus = AccountStatus.Suspended;

        _studentRepository.Student = student;

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetPublishedAsync("student-user"));

        // Assert
        Assert.Equal("Your student account is not active.", exception.Message);
    }

    [Fact]
    public async Task GetPublishedAsync_ShouldThrowException_WhenCollegeIsNotApproved()
    {
        // Arrange
        var student = CreateStudent();
        student.College.Status = CollegeStatus.Pending;

        _studentRepository.Student = student;

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetPublishedAsync("student-user"));

        // Assert
        Assert.Equal("Your college is not approved.", exception.Message);
    }

    [Fact]
    public async Task GetPublishedAsync_ShouldNotReturnJob_WhenDepartmentDoesNotMatch()
    {
        // Arrange
        var student = CreateStudent();
        _studentRepository.Student = student;

        var job = CreateJob(1);

        job.TargetColleges.Add(new JobTargetCollege
        {
            CollegeId = student.CollegeId
        });

        job.EligibleDepartments.Add(new JobEligibleDepartment
        {
            DepartmentName = "ECE"
        });

        job.EligibleGraduationYears.Add(new JobEligibleGraduationYear
        {
            GraduationYear = 2027
        });

        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.GetPublishedAsync("student-user");

        // Assert
        Assert.Empty(result);
    }


    // ------------------------------------
    // CreateAsync Tests
    // ------------------------------------

    [Fact]
    public async Task CreateAsync_ShouldCreateJob_WhenRequestIsValid()
    {
        // Arrange
        SetupActiveCompanyHR();
        await SeedApprovedCollegeAsync();

        var request = CreateJobRequest();

        // Act
        var result = await _service.CreateAsync(request, "hr-user");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Software Developer", result.JobTitle);
        Assert.Equal(10, result.CompanyId);
        Assert.Equal(JobStatus.Draft, result.Status);
        Assert.Single(result.TargetCollegeIds);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenHRDoesNotExist()
    {
        // Arrange
        var request = CreateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(request, "unknown-user"));

        // Assert
        Assert.Equal("Company HR profile was not found.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenHRAccountIsInactive()
    {
        // Arrange
        SetupActiveCompanyHR();
        _companyHRRepository.CompanyHR!.User.AccountStatus =
            AccountStatus.Suspended;

        var request = CreateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(request, "hr-user"));

        // Assert
        Assert.Equal("Your Company HR account is not active.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenCompanyIsNotApproved()
    {
        // Arrange
        SetupActiveCompanyHR();
        _companyHRRepository.CompanyHR!.Company.Status =
            CompanyStatus.Pending;

        var request = CreateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(request, "hr-user"));

        // Assert
        Assert.Equal("Your company is not approved.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenTargetCollegeIsNotApproved()
    {
        // Arrange
        SetupActiveCompanyHR();

        _context.Colleges.Add(new College
        {
            Id = 5,
            CollegeName = "Test College",
            Status = CollegeStatus.Pending
        });

        await _context.SaveChangesAsync();

        var request = CreateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(request, "hr-user"));

        // Assert
        Assert.Equal(
            "One or more target colleges do not exist or are not approved.",
            exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenDeadlineIsExpired()
    {
        // Arrange
        SetupActiveCompanyHR();
        await SeedApprovedCollegeAsync();

        var request = CreateJobRequest();
        request.ApplicationDeadline = DateTime.UtcNow.AddDays(-1);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(request, "hr-user"));

        // Assert
        Assert.Equal(
            "Application deadline must be in the future.",
            exception.Message);
    }

    // ------------------------------------
    // UpdateAsync Tests
    // ------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldUpdateJob_WhenRequestIsValid()
    {
        // Arrange
        SetupActiveCompanyHR();
        await SeedApprovedCollegeAsync();

        var job = CreateJob(1);
        job.TargetColleges.Add(new JobTargetCollege { CollegeId = 5 });
        job.EligibleDepartments.Add(
            new JobEligibleDepartment { DepartmentName = "CSE" });
        job.EligibleGraduationYears.Add(
            new JobEligibleGraduationYear { GraduationYear = 2027 });

        _jobRepository.Jobs.Add(job);

        var request = CreateUpdateJobRequest();
        request.JobTitle = "Senior Software Developer";

        // Act
        var result = await _service.UpdateAsync(1, request, "hr-user");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Senior Software Developer", result.JobTitle);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenJobDoesNotExist()
    {
        // Arrange
        SetupActiveCompanyHR();

        var request = CreateUpdateJobRequest();

        // Act
        var result = await _service.UpdateAsync(100, request, "hr-user");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenJobBelongsToAnotherCompany()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1, companyId: 20);
        _jobRepository.Jobs.Add(job);

        var request = CreateUpdateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateAsync(1, request, "hr-user"));

        // Assert
        Assert.Equal(
            "You cannot manage another company's job.",
            exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenJobIsNotDraft()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1);
        job.Status = JobStatus.Published;

        _jobRepository.Jobs.Add(job);

        var request = CreateUpdateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request, "hr-user"));

        // Assert
        Assert.Equal("Only draft jobs can be updated.", exception.Message);
    }

    // ------------------------------------
    // PublishAsync Tests
    // ------------------------------------

    [Fact]
    public async Task PublishAsync_ShouldPublishJob_WhenJobIsDraft()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1);
        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.PublishAsync(1, "hr-user");

        // Assert
        Assert.True(result);
        Assert.Equal(JobStatus.Published, job.Status);
        Assert.NotNull(job.UpdatedAt);
    }

    [Fact]
    public async Task PublishAsync_ShouldReturnFalse_WhenJobDoesNotExist()
    {
        // Arrange
        SetupActiveCompanyHR();

        // Act
        var result = await _service.PublishAsync(100, "hr-user");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task PublishAsync_ShouldThrowException_WhenDeadlineIsExpired()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1);
        job.ApplicationDeadline = DateTime.UtcNow.AddDays(-1);

        _jobRepository.Jobs.Add(job);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.PublishAsync(1, "hr-user"));

        // Assert
        Assert.Equal(
            "Cannot publish a job with an expired application deadline.",
            exception.Message);
    }

    // ------------------------------------
    // CloseAsync Tests
    // ------------------------------------

    [Fact]
    public async Task CloseAsync_ShouldCloseJob_WhenJobIsPublished()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1);
        job.Status = JobStatus.Published;

        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.CloseAsync(1, "hr-user");

        // Assert
        Assert.True(result);
        Assert.Equal(JobStatus.Closed, job.Status);
        Assert.NotNull(job.UpdatedAt);
    }

    [Fact]
    public async Task CloseAsync_ShouldThrowException_WhenJobIsNotPublished()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1);
        job.Status = JobStatus.Draft;

        _jobRepository.Jobs.Add(job);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CloseAsync(1, "hr-user"));

        // Assert
        Assert.Equal("Only published jobs can be closed.", exception.Message);
    }

    // ------------------------------------
    // GetByCompanyUserIdAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetByCompanyUserIdAsync_ShouldReturnJobsForHRCompany()
    {
        // Arrange
        SetupActiveCompanyHR();

        _jobRepository.Jobs.Add(CreateJob(1, companyId: 10));
        _jobRepository.Jobs.Add(CreateJob(2, companyId: 20));

        // Act
        var result = await _service.GetByCompanyUserIdAsync("hr-user");

        // Assert
        var jobs = result.ToList();

        Assert.Single(jobs);
        Assert.Equal(10, jobs[0].CompanyId);
    }

    // ------------------------------------
    // Additional Helper Methods
    // ------------------------------------

    private void SetupActiveCompanyHR()
    {
        _companyHRRepository.CompanyHR = new CompanyHR
        {
            Id = 1,
            UserId = "hr-user",
            CompanyId = 10,

            User = new ApplicationUser
            {
                Id = "hr-user",
                AccountStatus = AccountStatus.Active
            },

            Company = new Company
            {
                Id = 10,
                CompanyName = "ABC Technologies",
                Status = CompanyStatus.Approved
            }
        };
    }

    private async Task SeedApprovedCollegeAsync()
    {
        _context.Colleges.Add(new College
        {
            Id = 5,
            CollegeName = "Test Engineering College",
            Status = CollegeStatus.Approved
        });

        await _context.SaveChangesAsync();
    }

    private static CreateJobRequestDto CreateJobRequest()
    {
        return new CreateJobRequestDto
        {
            JobTitle = "Software Developer",
            JobDescription = "Develop software applications.",
            RequiredSkills = "C#, SQL",
            JobLocation = "Chennai",
            Salary = 500000,
            ApplicationDeadline = DateTime.UtcNow.AddDays(10),
            TargetCollegeIds = new List<int> { 5 },
            EligibleDepartments = new List<string> { "CSE" },
            EligibleGraduationYears = new List<int> { 2027 }
        };
    }

    private static UpdateJobRequestDto CreateUpdateJobRequest()
    {
        return new UpdateJobRequestDto
        {
            JobTitle = "Software Developer",
            JobDescription = "Develop software applications.",
            RequiredSkills = "C#, SQL",
            JobLocation = "Chennai",
            Salary = 600000,
            ApplicationDeadline = DateTime.UtcNow.AddDays(15),
            TargetCollegeIds = new List<int> { 5 },
            EligibleDepartments = new List<string> { "CSE" },
            EligibleGraduationYears = new List<int> { 2027 }
        };
    }


    // ------------------------------------
    // Additional UpdateAsync Tests
    // ------------------------------------

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenDeadlineIsExpired()
    {
        // Arrange
        SetupActiveCompanyHR();
        await SeedApprovedCollegeAsync();

        _jobRepository.Jobs.Add(CreateJob(1));

        var request = CreateUpdateJobRequest();
        request.ApplicationDeadline = DateTime.UtcNow.AddDays(-1);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request, "hr-user"));

        // Assert
        Assert.Equal(
            "Application deadline must be in the future.",
            exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenTargetCollegeIsNotApproved()
    {
        // Arrange
        SetupActiveCompanyHR();

        _context.Colleges.Add(new College
        {
            Id = 5,
            CollegeName = "Pending College",
            Status = CollegeStatus.Pending
        });

        await _context.SaveChangesAsync();

        _jobRepository.Jobs.Add(CreateJob(1));

        var request = CreateUpdateJobRequest();

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request, "hr-user"));

        // Assert
        Assert.Equal(
            "One or more target colleges do not exist or are not approved.",
            exception.Message);
    }

    // ------------------------------------
    // Additional PublishAsync Tests
    // ------------------------------------

    [Fact]
    public async Task PublishAsync_ShouldThrowException_WhenJobBelongsToAnotherCompany()
    {
        // Arrange
        SetupActiveCompanyHR();

        _jobRepository.Jobs.Add(CreateJob(1, companyId: 20));

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.PublishAsync(1, "hr-user"));

        // Assert
        Assert.Equal(
            "You cannot manage another company's job.",
            exception.Message);
    }

    [Fact]
    public async Task PublishAsync_ShouldThrowException_WhenJobIsNotDraft()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1);
        job.Status = JobStatus.Published;

        _jobRepository.Jobs.Add(job);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.PublishAsync(1, "hr-user"));

        // Assert
        Assert.Equal(
            "Only draft jobs can be published.",
            exception.Message);
    }

    // ------------------------------------
    // Additional CloseAsync Tests
    // ------------------------------------

    [Fact]
    public async Task CloseAsync_ShouldReturnFalse_WhenJobDoesNotExist()
    {
        // Arrange
        SetupActiveCompanyHR();

        // Act
        var result = await _service.CloseAsync(100, "hr-user");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CloseAsync_ShouldThrowException_WhenJobBelongsToAnotherCompany()
    {
        // Arrange
        SetupActiveCompanyHR();

        var job = CreateJob(1, companyId: 20);
        job.Status = JobStatus.Published;

        _jobRepository.Jobs.Add(job);

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CloseAsync(1, "hr-user"));

        // Assert
        Assert.Equal(
            "You cannot manage another company's job.",
            exception.Message);
    }

    // ------------------------------------
    // Additional GetPublishedAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetPublishedAsync_ShouldNotReturnJob_WhenGraduationYearDoesNotMatch()
    {
        // Arrange
        var student = CreateStudent();
        _studentRepository.Student = student;

        var job = CreateJob(1);

        job.TargetColleges.Add(new JobTargetCollege
        {
            CollegeId = student.CollegeId
        });

        job.EligibleDepartments.Add(new JobEligibleDepartment
        {
            DepartmentName = student.Department
        });

        job.EligibleGraduationYears.Add(new JobEligibleGraduationYear
        {
            GraduationYear = 2026
        });

        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.GetPublishedAsync("student-user");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPublishedAsync_ShouldNotReturnJob_WhenCollegeDoesNotMatch()
    {
        // Arrange
        var student = CreateStudent();
        _studentRepository.Student = student;

        var job = CreateJob(1);

        job.TargetColleges.Add(new JobTargetCollege
        {
            CollegeId = 99
        });

        job.EligibleDepartments.Add(new JobEligibleDepartment
        {
            DepartmentName = student.Department
        });

        job.EligibleGraduationYears.Add(new JobEligibleGraduationYear
        {
            GraduationYear = student.GraduationYear
        });

        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.GetPublishedAsync("student-user");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPublishedAsync_ShouldNotReturnJob_WhenDeadlineHasExpired()
    {
        // Arrange
        var student = CreateStudent();
        _studentRepository.Student = student;

        var job = CreateJob(1);
        job.ApplicationDeadline = DateTime.UtcNow.AddDays(-1);

        job.TargetColleges.Add(new JobTargetCollege
        {
            CollegeId = student.CollegeId
        });

        job.EligibleDepartments.Add(new JobEligibleDepartment
        {
            DepartmentName = student.Department
        });

        job.EligibleGraduationYears.Add(new JobEligibleGraduationYear
        {
            GraduationYear = student.GraduationYear
        });

        _jobRepository.Jobs.Add(job);

        // Act
        var result = await _service.GetPublishedAsync("student-user");

        // Assert
        Assert.Empty(result);
    }

    // ------------------------------------
    // Additional GetByCompanyUserIdAsync Tests
    // ------------------------------------

    [Fact]
    public async Task GetByCompanyUserIdAsync_ShouldThrowException_WhenHRDoesNotExist()
    {
        // Arrange
        _companyHRRepository.CompanyHR = null;

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetByCompanyUserIdAsync("unknown-user"));

        // Assert
        Assert.Equal(
            "Company HR profile was not found.",
            exception.Message);
    }



    // ------------------------------------
    // Helper Methods
    // ------------------------------------

    private static JobOpening CreateJob(
        int id,
        int companyId = 10)
    {
        return new JobOpening
        {
            Id = id,
            CompanyId = companyId,
            JobTitle = "Software Developer",
            JobDescription = "Develop software applications.",
            RequiredSkills = "C#, SQL",
            JobLocation = "Chennai",
            Salary = 500000,
            ApplicationDeadline = DateTime.UtcNow.AddDays(10),
            Status = JobStatus.Draft,
            CreatedAt = DateTime.UtcNow,

            Company = new Company
            {
                Id = companyId,
                CompanyName = "ABC Technologies",
                Status = CompanyStatus.Approved
            }
        };
    }

    private static Student CreateStudent()
    {
        return new Student
        {
            Id = 1,
            UserId = "student-user",
            CollegeId = 5,
            RegisterNumber = "REG001",
            Department = "CSE",
            GraduationYear = 2027,

            User = new ApplicationUser
            {
                Id = "student-user",
                AccountStatus = AccountStatus.Active
            },

            College = new College
            {
                Id = 5,
                CollegeName = "ABC Engineering College",
                Status = CollegeStatus.Approved
            }
        };
    }

    // ------------------------------------
    // Fake Job Repository
    // ------------------------------------

    private class FakeJobRepository : IJobRepository
    {
        public List<JobOpening> Jobs { get; } = new();

        public Task<IEnumerable<JobOpening>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<JobOpening>>(Jobs);
        }

        public Task<JobOpening?> GetByIdAsync(int id)
        {
            return Task.FromResult(
                Jobs.FirstOrDefault(job => job.Id == id));
        }

        public Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId)
        {
            return Task.FromResult<IEnumerable<JobOpening>>(
                Jobs.Where(job => job.CompanyId == companyId).ToList());
        }

        public Task<IEnumerable<JobOpening>> GetPublishedAsync()
        {
            return Task.FromResult<IEnumerable<JobOpening>>(
                Jobs.Where(job => job.Status == JobStatus.Published).ToList());
        }

        public Task AddAsync(JobOpening job)
        {
            job.Id = Jobs.Count == 0 ? 1 : Jobs.Max(j => j.Id) + 1;
            job.Company = new Company
            {
                Id = job.CompanyId,
                CompanyName = "ABC Technologies",
                Status = CompanyStatus.Approved
            };

            Jobs.Add(job);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(JobOpening job)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(Jobs.Any(job => job.Id == id));
        }
    }

    // ------------------------------------
    // Fake Company HR Repository
    // ------------------------------------

    private class FakeCompanyHRRepository : ICompanyHRRepository
    {
        public CompanyHR? CompanyHR { get; set; }

        public Task<IEnumerable<CompanyHR>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CompanyHR>>(
                CompanyHR == null ? new List<CompanyHR>() : new List<CompanyHR> { CompanyHR });
        }

        public Task<CompanyHR?> GetByIdAsync(int id)
        {
            return Task.FromResult(CompanyHR?.Id == id ? CompanyHR : null);
        }

        public Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId)
        {
            return Task.FromResult<IEnumerable<CompanyHR>>(
                CompanyHR?.CompanyId == companyId
                    ? new List<CompanyHR> { CompanyHR }
                    : new List<CompanyHR>());
        }

        public Task<CompanyHR?> GetByUserIdAsync(string userId)
        {
            return Task.FromResult(
                CompanyHR?.UserId == userId ? CompanyHR : null);
        }

        public Task AddAsync(CompanyHR companyHR)
        {
            CompanyHR = companyHR;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CompanyHR companyHR)
        {
            CompanyHR = companyHR;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(CompanyHR?.Id == id);
        }
    }

    // ------------------------------------
    // Fake Student Repository
    // ------------------------------------

    private class FakeStudentRepository : IStudentRepository
    {
        public Student? Student { get; set; }

        public Task<IEnumerable<Student>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Student>>(
                Student == null ? new List<Student>() : new List<Student> { Student });
        }

        public Task<Student?> GetByIdAsync(int id)
        {
            return Task.FromResult(Student?.Id == id ? Student : null);
        }

        public Task<Student?> GetByUserIdAsync(string userId)
        {
            return Task.FromResult(
                Student?.UserId == userId ? Student : null);
        }

        public Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId)
        {
            return Task.FromResult<IEnumerable<Student>>(
                Student?.CollegeId == collegeId
                    ? new List<Student> { Student }
                    : new List<Student>());
        }

        public Task<IEnumerable<Student>> GetPendingAsync(int collegeId)
        {
            return Task.FromResult<IEnumerable<Student>>(
                new List<Student>());
        }

        public Task AddAsync(Student student)
        {
            Student = student;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Student student)
        {
            Student = student;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(Student?.Id == id);
        }

        public Task<bool> RegisterNumberExistsAsync(string registerNumber)
        {
            return Task.FromResult(
                Student?.RegisterNumber == registerNumber);
        }
    }
}
