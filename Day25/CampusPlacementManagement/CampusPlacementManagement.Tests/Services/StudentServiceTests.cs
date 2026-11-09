
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CampusPlacementManagement.Tests.Services;

public class StudentServiceTests
{
    private readonly FakeStudentRepository _studentRepository;
    private readonly FakePlacementOfficerRepository _officerRepository;
    private readonly FakeUserManager _userManager;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _studentRepository = new FakeStudentRepository();
        _officerRepository = new FakePlacementOfficerRepository();
        _userManager = new FakeUserManager();

        // The context is not needed by the retrieval,
        // approval, rejection, or enrollment-validation tests.
        _service = new StudentService(
            _studentRepository,
            _officerRepository,
            _userManager,
            null!);
    }

    // -------------------------------
    // GET ALL STUDENTS
    // -------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudents()
    {
        _studentRepository.Students.Add(CreateStudent(1));
        _studentRepository.Students.Add(CreateStudent(2));

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    // -------------------------------
    // GET STUDENT BY ID
    // -------------------------------

    [Fact]
    public async Task GetByIdAsync_WhenStudentExists_ShouldReturnStudent()
    {
        _studentRepository.Students.Add(CreateStudent(1));

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("REG001", result.RegisterNumber);
    }

    [Fact]
    public async Task GetByIdAsync_WhenStudentDoesNotExist_ShouldReturnNull()
    {
        var result = await _service.GetByIdAsync(100);

        Assert.Null(result);
    }

    // -------------------------------
    // GET STUDENTS BY COLLEGE
    // -------------------------------

    [Fact]
    public async Task GetByCollegeIdAsync_ShouldReturnStudentsOfCollege()
    {
        _studentRepository.Students.Add(CreateStudent(1, collegeId: 1));
        _studentRepository.Students.Add(CreateStudent(2, collegeId: 2));

        var result = await _service.GetByCollegeIdAsync(1);

        Assert.Single(result);
        Assert.All(result, student => Assert.Equal(1, student.CollegeId));
    }

    // -------------------------------
    // GET PENDING STUDENTS
    // -------------------------------

    [Fact]
    public async Task GetPendingAsync_ShouldReturnOnlyPendingStudents()
    {
        var pendingStudent = CreateStudent(1);
        pendingStudent.User.AccountStatus = AccountStatus.Pending;

        var activeStudent = CreateStudent(2);
        activeStudent.User.AccountStatus = AccountStatus.Active;

        _studentRepository.Students.Add(pendingStudent);
        _studentRepository.Students.Add(activeStudent);

        var result = await _service.GetPendingAsync(1);

        Assert.Single(result);
        Assert.Equal("REG001", result.First().RegisterNumber);
    }

    // -------------------------------
    // ENROLLMENT VALIDATION
    // -------------------------------

    [Fact]
    public async Task EnrollAsync_WhenOfficerDoesNotExist_ShouldThrowUnauthorizedAccessException()
    {
        var request = CreateStudentRequest();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.EnrollAsync(request, "invalid-officer"));
    }

    [Fact]
    public async Task EnrollAsync_WhenOfficerIsInactive_ShouldThrowUnauthorizedAccessException()
    {
        var officer = CreateOfficer();
        officer.User.AccountStatus = AccountStatus.Pending;

        _officerRepository.Officers.Add(officer);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.EnrollAsync(CreateStudentRequest(), "officer-1"));
    }

    [Fact]
    public async Task EnrollAsync_WhenCollegeIsNotApproved_ShouldThrowInvalidOperationException()
    {
        var officer = CreateOfficer();
        officer.College.Status = CollegeStatus.Pending;

        _officerRepository.Officers.Add(officer);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.EnrollAsync(CreateStudentRequest(), "officer-1"));
    }

    [Fact]
    public async Task EnrollAsync_WhenRegisterNumberAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var officer = CreateOfficer();
        _officerRepository.Officers.Add(officer);

        _studentRepository.ExistingRegisterNumbers.Add("REG001");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.EnrollAsync(CreateStudentRequest(), "officer-1"));
    }

    [Fact]
    public async Task EnrollAsync_WhenEmailAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var officer = CreateOfficer();
        _officerRepository.Officers.Add(officer);

        _userManager.ExistingEmails.Add("student@example.com");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.EnrollAsync(CreateStudentRequest(), "officer-1"));
    }

    // -------------------------------
    // APPROVE STUDENT
    // -------------------------------

    [Fact]
    public async Task ApproveAsync_WhenOfficerDoesNotExist_ShouldThrowUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.ApproveAsync(1, "invalid-officer"));
    }

    [Fact]
    public async Task ApproveAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var result = await _service.ApproveAsync(100, "officer-1");

        Assert.False(result);
    }

    [Fact]
    public async Task ApproveAsync_WhenStudentBelongsToAnotherCollege_ShouldThrowUnauthorizedAccessException()
    {
        _officerRepository.Officers.Add(CreateOfficer(collegeId: 1));
        _studentRepository.Students.Add(CreateStudent(1, collegeId: 2));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.ApproveAsync(1, "officer-1"));
    }

    [Fact]
    public async Task ApproveAsync_WhenStudentIsNotPending_ShouldThrowInvalidOperationException()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var student = CreateStudent(1);
        student.User.AccountStatus = AccountStatus.Active;

        _studentRepository.Students.Add(student);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApproveAsync(1, "officer-1"));
    }

    [Fact]
    public async Task ApproveAsync_WhenStudentIsPending_ShouldActivateStudent()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var student = CreateStudent(1);
        student.User.AccountStatus = AccountStatus.Pending;

        _studentRepository.Students.Add(student);

        var result = await _service.ApproveAsync(1, "officer-1");

        Assert.True(result);
        Assert.Equal(AccountStatus.Active, student.User.AccountStatus);
        Assert.Contains("Student", _userManager.AddedRoles);
        Assert.Contains(student, _studentRepository.UpdatedStudents);
    }

    // -------------------------------
    // REJECT STUDENT
    // -------------------------------

    [Fact]
    public async Task RejectAsync_WhenOfficerDoesNotExist_ShouldThrowUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.RejectAsync(1, "invalid-officer"));
    }

    [Fact]
    public async Task RejectAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var result = await _service.RejectAsync(100, "officer-1");

        Assert.False(result);
    }

    [Fact]
    public async Task RejectAsync_WhenStudentBelongsToAnotherCollege_ShouldThrowUnauthorizedAccessException()
    {
        _officerRepository.Officers.Add(CreateOfficer(collegeId: 1));
        _studentRepository.Students.Add(CreateStudent(1, collegeId: 2));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.RejectAsync(1, "officer-1"));
    }

    [Fact]
    public async Task RejectAsync_WhenStudentIsNotPending_ShouldThrowInvalidOperationException()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var student = CreateStudent(1);
        student.User.AccountStatus = AccountStatus.Active;

        _studentRepository.Students.Add(student);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.RejectAsync(1, "officer-1"));
    }

    [Fact]
    public async Task RejectAsync_WhenStudentIsPending_ShouldRejectStudent()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var student = CreateStudent(1);
        student.User.AccountStatus = AccountStatus.Pending;

        _studentRepository.Students.Add(student);

        var result = await _service.RejectAsync(1, "officer-1");

        Assert.True(result);
        Assert.Equal(AccountStatus.Rejected, student.User.AccountStatus);
    }

    // -------------------------------
    // GET PENDING STUDENTS FOR OFFICER
    // -------------------------------

    [Fact]
    public async Task GetPendingStudentsForOfficerAsync_WhenOfficerDoesNotExist_ShouldThrowUnauthorizedAccessException()
    {
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.GetPendingStudentsForOfficerAsync("invalid-officer"));
    }

    [Fact]
    public async Task GetPendingStudentsForOfficerAsync_WhenCollegeIsNotApproved_ShouldThrowInvalidOperationException()
    {
        var officer = CreateOfficer();
        officer.College.Status = CollegeStatus.Pending;

        _officerRepository.Officers.Add(officer);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.GetPendingStudentsForOfficerAsync("officer-1"));
    }

    [Fact]
    public async Task GetPendingStudentsForOfficerAsync_WhenCollegeIsApproved_ShouldReturnPendingStudents()
    {
        _officerRepository.Officers.Add(CreateOfficer());

        var student = CreateStudent(1);
        student.User.AccountStatus = AccountStatus.Pending;

        _studentRepository.Students.Add(student);

        var result = await _service.GetPendingStudentsForOfficerAsync("officer-1");

        Assert.Single(result);
        Assert.Equal("REG001", result.First().RegisterNumber);
    }

    // -------------------------------
    // TEST DATA HELPERS
    // -------------------------------

    private static Student CreateStudent(int id, int collegeId = 1)
    {
        return new Student
        {
            Id = id,
            UserId = $"student-user-{id}",
            CollegeId = collegeId,
            RegisterNumber = $"REG{id:D3}",
            Department = "Computer Science",
            GraduationYear = 2027,
            CreatedAt = DateTime.UtcNow,
            User = new ApplicationUser
            {
                Id = $"student-user-{id}",
                Email = $"student{id}@example.com",
                UserName = $"student{id}@example.com",
                PhoneNumber = "9876543210",
                AccountStatus = AccountStatus.Pending
            },
            College = new College
            {
                Id = collegeId,
                CollegeName = $"College {collegeId}",
                CollegeCode = $"COL{collegeId}",
                Status = CollegeStatus.Approved
            }
        };
    }

    private static PlacementOfficer CreateOfficer(int collegeId = 1)
    {
        return new PlacementOfficer
        {
            Id = 1,
            UserId = "officer-1",
            CollegeId = collegeId,
            User = new ApplicationUser
            {
                Id = "officer-1",
                Email = "officer@example.com",
                UserName = "officer@example.com",
                AccountStatus = AccountStatus.Active
            },
            College = new College
            {
                Id = collegeId,
                CollegeName = $"College {collegeId}",
                CollegeCode = $"COL{collegeId}",
                Status = CollegeStatus.Approved
            }
        };
    }

    private static CreateStudentRequestDto CreateStudentRequest()
    {
        return new CreateStudentRequestDto
        {
            Email = "student@example.com",
            Password = "Student@123",
            PhoneNumber = "9876543210",
            RegisterNumber = "REG001",
            Department = "Computer Science",
            GraduationYear = 2027,
            ResumeUrl = "https://example.com/resume.pdf"
        };
    }

    // -------------------------------
    // FAKE STUDENT REPOSITORY
    // -------------------------------

    private class FakeStudentRepository : IStudentRepository
    {
        public List<Student> Students { get; } = new();

        public HashSet<string> ExistingRegisterNumbers { get; } = new();

        public List<Student> UpdatedStudents { get; } = new();

        public Task<IEnumerable<Student>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Student>>(Students);
        }

        public Task<Student?> GetByIdAsync(int id)
        {
            return Task.FromResult(Students.FirstOrDefault(s => s.Id == id));
        }

        public Task<Student?> GetByUserIdAsync(string userId)
        {
            return Task.FromResult(
                Students.FirstOrDefault(s => s.UserId == userId));
        }

        public Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId)
        {
            return Task.FromResult<IEnumerable<Student>>(
                Students.Where(s => s.CollegeId == collegeId).ToList());
        }

        public Task<IEnumerable<Student>> GetPendingAsync(int collegeId)
        {
            return Task.FromResult<IEnumerable<Student>>(
                Students.Where(s =>
                    s.CollegeId == collegeId &&
                    s.User.AccountStatus == AccountStatus.Pending).ToList());
        }

        public Task AddAsync(Student student)
        {
            student.Id = Students.Count == 0
                ? 1
                : Students.Max(s => s.Id) + 1;

            Students.Add(student);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Student student)
        {
            UpdatedStudents.Add(student);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(Students.Any(s => s.Id == id));
        }

        public Task<bool> RegisterNumberExistsAsync(string registerNumber)
        {
            return Task.FromResult(
                ExistingRegisterNumbers.Contains(registerNumber) ||
                Students.Any(s => s.RegisterNumber == registerNumber));
        }
    }

    // -------------------------------
    // FAKE PLACEMENT OFFICER REPOSITORY
    // -------------------------------

    private class FakePlacementOfficerRepository : IPlacementOfficerRepository
    {
        public List<PlacementOfficer> Officers { get; } = new();

        public Task<IEnumerable<PlacementOfficer>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<PlacementOfficer>>(Officers);
        }

        public Task<PlacementOfficer?> GetByIdAsync(int id)
        {
            return Task.FromResult(
                Officers.FirstOrDefault(o => o.Id == id));
        }

        public Task<PlacementOfficer?> GetByUserIdAsync(string userId)
        {
            return Task.FromResult(
                Officers.FirstOrDefault(o => o.UserId == userId));
        }

        public Task<IEnumerable<PlacementOfficer>> GetByCollegeIdAsync(int collegeId)
        {
            return Task.FromResult<IEnumerable<PlacementOfficer>>(
                Officers.Where(o => o.CollegeId == collegeId).ToList());
        }

        public Task AddAsync(PlacementOfficer placementOfficer)
        {
            Officers.Add(placementOfficer);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PlacementOfficer placementOfficer)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(Officers.Any(o => o.Id == id));
        }
    }

    // -------------------------------
    // FAKE USER MANAGER
    // -------------------------------

    private class FakeUserManager : UserManager<ApplicationUser>
    {
        public HashSet<string> ExistingEmails { get; } =
            new(StringComparer.OrdinalIgnoreCase);

        public List<string> AddedRoles { get; } = new();

        public FakeUserManager()
            : base(
                new FakeUserStore(),
                Microsoft.Extensions.Options.Options.Create(new IdentityOptions()),
                new PasswordHasher<ApplicationUser>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null!,
                new Logger<FakeUserManager>(new LoggerFactory()))
        {
        }

        public override Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            if (ExistingEmails.Contains(email))
            {
                return Task.FromResult<ApplicationUser?>(
                    new ApplicationUser
                    {
                        Id = "existing-user",
                        Email = email,
                        UserName = email
                    });
            }

            return Task.FromResult<ApplicationUser?>(null);
        }

        public override Task<IdentityResult> CreateAsync(
            ApplicationUser user,
            string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> AddToRoleAsync(
            ApplicationUser user,
            string role)
        {
            AddedRoles.Add(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        private class FakeUserStore : IUserStore<ApplicationUser>
        {
            public Task<IdentityResult> CreateAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
                => Task.FromResult(IdentityResult.Success);

            public Task<IdentityResult> UpdateAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
                => Task.FromResult(IdentityResult.Success);

            public Task<IdentityResult> DeleteAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
                => Task.FromResult(IdentityResult.Success);

            public Task<ApplicationUser?> FindByIdAsync(
                string userId,
                CancellationToken cancellationToken)
                => Task.FromResult<ApplicationUser?>(null);

            public Task<ApplicationUser?> FindByNameAsync(
                string normalizedUserName,
                CancellationToken cancellationToken)
                => Task.FromResult<ApplicationUser?>(null);

            public Task<string> GetUserIdAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
                => Task.FromResult(user.Id);

            public Task<string?> GetUserNameAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
                => Task.FromResult(user.UserName);

            public Task SetUserNameAsync(
                ApplicationUser user,
                string? userName,
                CancellationToken cancellationToken)
            {
                user.UserName = userName;
                return Task.CompletedTask;
            }

            public Task<string?> GetNormalizedUserNameAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
                => Task.FromResult(user.NormalizedUserName);

            public Task SetNormalizedUserNameAsync(
                ApplicationUser user,
                string? normalizedName,
                CancellationToken cancellationToken)
            {
                user.NormalizedUserName = normalizedName;
                return Task.CompletedTask;
            }

            public void Dispose()
            {
            }
        }
    }
}
