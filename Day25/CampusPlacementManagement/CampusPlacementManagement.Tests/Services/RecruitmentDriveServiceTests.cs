
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.RecruitmentDrives;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.EntityFrameworkCore;


namespace CampusPlacementManagement.Tests.Services;

public class RecruitmentDriveServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly RecruitmentDriveService _service;

    public RecruitmentDriveServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new RecruitmentDriveService(_context);
    }

    // ==========================================
    // BATCH 1: GET ALL RECRUITMENT DRIVES
    // ==========================================

    [Fact]
    public async Task GetAll_SuperAdmin_ReturnsAllDrives()
    {
        var company1 = CreateCompany(1);
        var company2 = CreateCompany(2);

        var college = CreateCollege(1);

        await _context.Companies.AddRangeAsync(company1, company2);
        await _context.Colleges.AddAsync(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company1, college);
        await AddDriveAsync(2, company2, college);

        var result = await _service.GetAllAsync(
            "admin-1", "SuperAdmin");

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAll_ValidCompanyHR_ReturnsOnlyOwnCompanyDrives()
    {
        var company1 = CreateCompany(1);
        var company2 = CreateCompany(2);
        var college = CreateCollege(1);

        await _context.Companies.AddRangeAsync(company1, company2);
        await _context.Colleges.AddAsync(college);

        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Users.AddAsync(user);

        await _context.CompanyHRs.AddAsync(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            CompanyId = company1.Id,
            User = user,
            Company = company1
        });

        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company1, college);
        await AddDriveAsync(2, company2, college);

        var result = await _service.GetAllAsync(
            "hr-1", "CompanyHR");

        Assert.Single(result);
        Assert.Equal(1, result[0].JobOpeningId);
    }

    [Fact]
    public async Task GetAll_CompanyHRNotFound_ThrowsUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("missing-hr", "CompanyHR"));
    }

    [Fact]
    public async Task GetAll_InactiveCompanyHR_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        var user = CreateUser("hr-1", AccountStatus.Inactive);

        await _context.Companies.AddAsync(company);
        await _context.Users.AddAsync(user);

        await _context.CompanyHRs.AddAsync(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            CompanyId = company.Id,
            User = user,
            Company = company
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("hr-1", "CompanyHR"));
    }

    [Fact]
    public async Task GetAll_UnapprovedCompany_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        company.Status = CompanyStatus.Pending;

        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Users.AddAsync(user);

        await _context.CompanyHRs.AddAsync(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            CompanyId = company.Id,
            User = user,
            Company = company
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("hr-1", "CompanyHR"));
    }

    [Fact]
    public async Task GetAll_InvalidRole_ThrowsUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("user-1", "UnknownRole"));
    }


    // ==========================================
    // BATCH 2: STUDENT ACCESS
    // ==========================================

    [Fact]
    public async Task GetAll_ValidStudent_ReturnsOwnCollegeDrives()
    {
        var company = CreateCompany(1);
        var college1 = CreateCollege(1);
        var college2 = CreateCollege(2);
        var user = CreateUser("student-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddRangeAsync(college1, college2);
        await _context.Users.AddAsync(user);

        _context.Students.Add(new Student
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college1.Id,
            College = college1
        });

        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college1);
        await AddDriveAsync(2, company, college2);

        var result = await _service.GetAllAsync(
            "student-1", "Student");

        Assert.Single(result);
        Assert.Equal(1, result[0].CollegeId);
    }

    [Fact]
    public async Task GetAll_StudentWithoutProfile_ThrowsUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("missing-student", "Student"));
    }

    [Fact]
    public async Task GetAll_InactiveStudent_ThrowsUnauthorizedAccessException()
    {
        var college = CreateCollege(1);
        var user = CreateUser("student-1", AccountStatus.Inactive);

        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.Students.Add(new Student
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college.Id,
            College = college
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("student-1", "Student"));
    }

    [Fact]
    public async Task GetAll_StudentFromUnapprovedCollege_ThrowsUnauthorizedAccessException()
    {
        var college = CreateCollege(1);
        college.Status = CollegeStatus.Pending;

        var user = CreateUser("student-1", AccountStatus.Active);

        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.Students.Add(new Student
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college.Id,
            College = college
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("student-1", "Student"));
    }

    // ==========================================
    // BATCH 2: PLACEMENT OFFICER ACCESS
    // ==========================================

    [Fact]
    public async Task GetAll_ValidPlacementOfficer_ReturnsOwnCollegeDrives()
    {
        var company = CreateCompany(1);
        var college1 = CreateCollege(1);
        var college2 = CreateCollege(2);
        var user = CreateUser("officer-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddRangeAsync(college1, college2);
        await _context.Users.AddAsync(user);

        _context.PlacementOfficers.Add(new PlacementOfficer
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college1.Id,
            College = college1
        });

        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college1);
        await AddDriveAsync(2, company, college2);

        var result = await _service.GetAllAsync(
            "officer-1", "PlacementOfficer");

        Assert.Single(result);
        Assert.Equal(1, result[0].CollegeId);
    }

    [Fact]
    public async Task GetAll_PlacementOfficerNotFound_ThrowsUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("missing-officer", "PlacementOfficer"));
    }

    [Fact]
    public async Task GetAll_InactivePlacementOfficer_ThrowsUnauthorizedAccessException()
    {
        var college = CreateCollege(1);
        var user = CreateUser("officer-1", AccountStatus.Inactive);

        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.PlacementOfficers.Add(new PlacementOfficer
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college.Id,
            College = college
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("officer-1", "PlacementOfficer"));
    }

    [Fact]
    public async Task GetAll_PlacementOfficerFromUnapprovedCollege_ThrowsUnauthorizedAccessException()
    {
        var college = CreateCollege(1);
        college.Status = CollegeStatus.Pending;

        var user = CreateUser("officer-1", AccountStatus.Active);

        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.PlacementOfficers.Add(new PlacementOfficer
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college.Id,
            College = college
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetAllAsync("officer-1", "PlacementOfficer"));
    }


    // ==========================================
    // BATCH 3: GET BY ID TESTS
    // ==========================================

    [Fact]
    public async Task GetById_SuperAdmin_ReturnsDrive()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddAsync(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college);

        var result = await _service.GetByIdAsync(
            1, "admin-1", "SuperAdmin");

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Campus Drive 1", result.DriveName);
    }

    [Fact]
    public async Task GetById_MissingDrive_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(
            100, "admin-1", "SuperAdmin");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetById_ValidCompanyHR_ReturnsOwnCompanyDrive()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);
        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CompanyId = company.Id,
            Company = company
        });

        await _context.SaveChangesAsync();
        await AddDriveAsync(1, company, college);

        var result = await _service.GetByIdAsync(
            1, "hr-1", "CompanyHR");

        Assert.NotNull(result);
        Assert.Equal(1, result.JobOpeningId);
    }

    [Fact]
    public async Task GetById_CompanyHRFromAnotherCompany_ThrowsUnauthorizedAccessException()
    {
        var company1 = CreateCompany(1);
        var company2 = CreateCompany(2);
        var college = CreateCollege(1);
        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Companies.AddRangeAsync(company1, company2);
        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CompanyId = company1.Id,
            Company = company1
        });

        await _context.SaveChangesAsync();
        await AddDriveAsync(1, company2, college);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetByIdAsync(1, "hr-1", "CompanyHR"));
    }

    [Fact]
    public async Task GetById_ValidStudent_ReturnsOwnCollegeDrive()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);
        var user = CreateUser("student-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.Students.Add(new Student
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college.Id,
            College = college
        });

        await _context.SaveChangesAsync();
        await AddDriveAsync(1, company, college);

        var result = await _service.GetByIdAsync(
            1, "student-1", "Student");

        Assert.NotNull(result);
        Assert.Equal(1, result.CollegeId);
    }

    [Fact]
    public async Task GetById_StudentFromAnotherCollege_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        var driveCollege = CreateCollege(1);
        var studentCollege = CreateCollege(2);
        var user = CreateUser("student-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddRangeAsync(
            driveCollege, studentCollege);
        await _context.Users.AddAsync(user);

        _context.Students.Add(new Student
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = studentCollege.Id,
            College = studentCollege
        });

        await _context.SaveChangesAsync();
        await AddDriveAsync(1, company, driveCollege);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetByIdAsync(1, "student-1", "Student"));
    }

    [Fact]
    public async Task GetById_ValidPlacementOfficer_ReturnsOwnCollegeDrive()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);
        var user = CreateUser("officer-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.PlacementOfficers.Add(new PlacementOfficer
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = college.Id,
            College = college
        });

        await _context.SaveChangesAsync();
        await AddDriveAsync(1, company, college);

        var result = await _service.GetByIdAsync(
            1, "officer-1", "PlacementOfficer");

        Assert.NotNull(result);
        Assert.Equal(1, result.CollegeId);
    }

    [Fact]
    public async Task GetById_PlacementOfficerFromAnotherCollege_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        var driveCollege = CreateCollege(1);
        var officerCollege = CreateCollege(2);
        var user = CreateUser("officer-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddRangeAsync(
            driveCollege, officerCollege);
        await _context.Users.AddAsync(user);

        _context.PlacementOfficers.Add(new PlacementOfficer
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CollegeId = officerCollege.Id,
            College = officerCollege
        });

        await _context.SaveChangesAsync();
        await AddDriveAsync(1, company, driveCollege);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetByIdAsync(
                1, "officer-1", "PlacementOfficer"));
    }

    [Fact]
    public async Task GetById_InvalidRole_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddAsync(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetByIdAsync(1, "user-1", "UnknownRole"));
    }


    // ==========================================
    // BATCH 4: CREATE RECRUITMENT DRIVE
    // ==========================================

    [Fact]
    public async Task Create_ValidRequest_CreatesRecruitmentDrive()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);
        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Colleges.AddAsync(college);
        await _context.Users.AddAsync(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CompanyId = company.Id,
            Company = company
        });

        var job = CreatePublishedJob(10, company);

        await _context.JobOpenings.AddAsync(job);
        await _context.SaveChangesAsync();

        _context.JobTargetColleges.Add(new JobTargetCollege
        {
            JobOpeningId = job.Id,
            CollegeId = college.Id
        });

        await _context.SaveChangesAsync();

        var request = new CreateRecruitmentDriveRequestDto
        {
            JobOpeningId = 10,
            CollegeId = 1,
            DriveName = "Campus Recruitment 2026",
            DriveDate = DateTime.UtcNow.AddDays(30),
            Venue = "Main Auditorium",
            Description = "Technical and HR interviews"
        };

        var result = await _service.CreateAsync(request, "hr-1");

        Assert.Equal("Campus Recruitment 2026", result.DriveName);
        Assert.Equal(job.Id, result.JobOpeningId);
        Assert.Equal(college.Id, result.CollegeId);
        Assert.Equal("Software Developer", result.JobTitle);
        Assert.Equal("College 1", result.CollegeName);
        Assert.Equal(RecruitmentDriveStatus.Scheduled, result.Status);

        Assert.Equal(1, await _context.RecruitmentDrives.CountAsync());
    }

    [Fact]
    public async Task Create_HRNotFound_ThrowsUnauthorizedAccessException()
    {
        var request = CreateValidCreateRequest();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(request, "missing-hr"));
    }

    [Fact]
    public async Task Create_InactiveHR_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        var user = CreateUser("hr-1", AccountStatus.Inactive);

        await _context.Companies.AddAsync(company);
        await _context.Users.AddAsync(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CompanyId = company.Id,
            Company = company
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    [Fact]
    public async Task Create_UnapprovedCompany_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        company.Status = CompanyStatus.Pending;

        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Users.AddAsync(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CompanyId = company.Id,
            Company = company
        });

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    [Fact]
    public async Task Create_JobNotFound_ThrowsKeyNotFoundException()
    {
        await SeedActiveHRAsync();

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    [Fact]
    public async Task Create_WrongCompanyJob_ThrowsUnauthorizedAccessException()
    {
        var company1 = CreateCompany(1);
        var company2 = CreateCompany(2);

        await SeedActiveHRAsync(company1);

        await _context.Companies.AddAsync(company2);

        await _context.JobOpenings.AddAsync(
            CreatePublishedJob(10, company2));

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    [Fact]
    public async Task Create_UnpublishedJob_ThrowsInvalidOperationException()
    {
        var company = CreateCompany(1);

        await SeedActiveHRAsync(company);

        var job = CreatePublishedJob(10, company);
        job.Status = JobStatus.Draft;

        await _context.JobOpenings.AddAsync(job);
        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    [Fact]
    public async Task Create_CollegeNotFound_ThrowsKeyNotFoundException()
    {
        var company = CreateCompany(1);

        await SeedActiveHRAsync(company);

        await _context.JobOpenings.AddAsync(
            CreatePublishedJob(10, company));

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    [Fact]
    public async Task Create_UnapprovedCollege_ThrowsInvalidOperationException()
    {
        var company = CreateCompany(1);
        var college = CreateCollege(1);
        college.Status = CollegeStatus.Pending;

        await SeedActiveHRAsync(company);

        await _context.Colleges.AddAsync(college);
        await _context.JobOpenings.AddAsync(
            CreatePublishedJob(10, company));

        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(CreateValidCreateRequest(), "hr-1"));
    }

    // ==========================================
    // BATCH 4 TEST HELPERS
    // ==========================================

    private CreateRecruitmentDriveRequestDto CreateValidCreateRequest()
    {
        return new CreateRecruitmentDriveRequestDto
        {
            JobOpeningId = 10,
            CollegeId = 1,
            DriveName = "Campus Drive",
            DriveDate = DateTime.UtcNow.AddDays(30),
            Venue = "Auditorium",
            Description = "Campus recruitment"
        };
    }

    private JobOpening CreatePublishedJob(int id, Company company)
    {
        return new JobOpening
        {
            Id = id,
            CompanyId = company.Id,
            Company = company,
            JobTitle = "Software Developer",
            JobDescription = "Software development position",
            RequiredSkills = "C#, ASP.NET Core",
            JobLocation = "Chennai",
            Salary = 500000,
            ApplicationDeadline = DateTime.UtcNow.AddDays(10),
            Status = JobStatus.Published
        };
    }

    private async Task SeedActiveHRAsync(Company? company = null)
    {
        company ??= CreateCompany(1);

        var user = CreateUser("hr-1", AccountStatus.Active);

        await _context.Companies.AddAsync(company);
        await _context.Users.AddAsync(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            Id = 1,
            UserId = user.Id,
            User = user,
            CompanyId = company.Id,
            Company = company
        });

        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task Update_ValidRequest_UpdatesRecruitmentDrive()
    {
        var company = CreateCompany(1);
        await SeedActiveHRAsync(company);

        var college = CreateCollege(1);
        _context.Colleges.Add(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college);

        var request = CreateValidUpdateRequest();

        var result = await _service.UpdateAsync(1, request, "hr-1");

        Assert.Equal(request.DriveName, result.DriveName);
        Assert.Equal(request.Venue, result.Venue);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DriveDate, result.DriveDate);

        var updatedDrive = await _context.RecruitmentDrives.FindAsync(1);
        Assert.Equal(request.DriveName, updatedDrive!.DriveName);
    }

    [Fact]
    public async Task Update_HRNotFound_ThrowsUnauthorizedAccessException()
    {
        var request = CreateValidUpdateRequest();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateAsync(1, request, "unknown-hr"));
    }

    [Fact]
    public async Task Update_InactiveHR_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        _context.Companies.Add(company);

        var user = CreateUser("hr-1", AccountStatus.Inactive);
        _context.Users.Add(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            UserId = user.Id,
            CompanyId = company.Id,
            User = user,
            Company = company
        });

        await _context.SaveChangesAsync();

        var request = CreateValidUpdateRequest();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task Update_UnapprovedCompany_ThrowsUnauthorizedAccessException()
    {
        var company = CreateCompany(1);
        company.Status = CompanyStatus.Pending;

        _context.Companies.Add(company);

        var user = CreateUser("hr-1", AccountStatus.Active);
        _context.Users.Add(user);

        _context.CompanyHRs.Add(new CompanyHR
        {
            UserId = user.Id,
            CompanyId = company.Id,
            User = user,
            Company = company
        });

        await _context.SaveChangesAsync();

        var request = CreateValidUpdateRequest();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task Update_DriveNotFound_ThrowsKeyNotFoundException()
    {
        var company = CreateCompany(1);
        await SeedActiveHRAsync(company);

        var request = CreateValidUpdateRequest();

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateAsync(999, request, "hr-1"));
    }

    [Fact]
    public async Task Update_DriveBelongsToAnotherCompany_ThrowsUnauthorizedAccessException()
    {
        var company1 = CreateCompany(1);
        await SeedActiveHRAsync(company1);

        var company2 = CreateCompany(2);
        var college = CreateCollege(1);

        _context.Companies.Add(company2);
        _context.Colleges.Add(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company2, college);

        var request = CreateValidUpdateRequest();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.UpdateAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task Update_CancelledDrive_ThrowsInvalidOperationException()
    {
        var company = CreateCompany(1);
        await SeedActiveHRAsync(company);

        var college = CreateCollege(1);
        _context.Colleges.Add(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college);

        var drive = await _context.RecruitmentDrives.FindAsync(1);
        drive!.Status = RecruitmentDriveStatus.Cancelled;
        await _context.SaveChangesAsync();

        var request = CreateValidUpdateRequest();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request, "hr-1"));
    }

    [Fact]
    public async Task Update_PastDriveDate_ThrowsInvalidOperationException()
    {
        var company = CreateCompany(1);
        await SeedActiveHRAsync(company);

        var college = CreateCollege(1);
        _context.Colleges.Add(college);
        await _context.SaveChangesAsync();

        await AddDriveAsync(1, company, college);

        var request = CreateValidUpdateRequest();
        request.DriveDate = DateTime.UtcNow.AddDays(-1);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request, "hr-1"));
    }


    // ==========================================
    // TEST DATA HELPERS
    // ==========================================

    private static ApplicationUser CreateUser(
        string userId,
        AccountStatus status)
    {
        return new ApplicationUser
        {
            Id = userId,
            UserName = userId,
            Email = $"{userId}@test.com",
            AccountStatus = status
        };
    }

    private static Company CreateCompany(int id)
    {
        return new Company
        {
            Id = id,
            CompanyName = $"Company {id}",
            Status = CompanyStatus.Approved
        };
    }

    private static College CreateCollege(int id)
    {
        return new College
        {
            Id = id,
            CollegeName = $"College {id}",
            CollegeCode = $"COL{id}",
            Status = CollegeStatus.Approved
        };
    }

    private async Task AddDriveAsync(
        int driveId,
        Company company,
        College college)
    {
        var job = new JobOpening
        {
            Id = driveId,
            CompanyId = company.Id,
            Company = company,
            JobTitle = $"Software Developer {driveId}",
            JobDescription = "Software development position",
            RequiredSkills = "C#, ASP.NET Core",
            JobLocation = "Chennai",
            Salary = 500000,
            ApplicationDeadline = DateTime.UtcNow.AddDays(10),
            Status = JobStatus.Published
        };

        var drive = new RecruitmentDrive
        {
            Id = driveId,
            JobOpeningId = job.Id,
            JobOpening = job,
            CollegeId = college.Id,
            College = college,
            DriveName = $"Campus Drive {driveId}",
            DriveDate = DateTime.UtcNow.AddDays(20),
            Venue = "Main Auditorium",
            Description = "Campus recruitment",
            Status = RecruitmentDriveStatus.Scheduled
        };

        await _context.RecruitmentDrives.AddAsync(drive);
        await _context.SaveChangesAsync();
    }
}
