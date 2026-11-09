
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CampusPlacementManagement.Tests.Services;

public class PlacementOfficerServiceTests
{
    private readonly FakePlacementOfficerRepository _repository;
    private readonly FakeUserManager _userManager;
    private readonly PlacementOfficerService _service;

    public PlacementOfficerServiceTests()
    {
        _repository = new FakePlacementOfficerRepository();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=CampusPlacementUnitTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        var context = new ApplicationDbContext(options);

        _userManager = new FakeUserManager(
            new UserStore<ApplicationUser>(context));

        _service = new PlacementOfficerService(
            _repository,
            collegeRepository: null!,
            _userManager,
            context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPlacementOfficers()
    {
        _repository.Officers.Add(CreateOfficer(1, "officer1@example.com"));
        _repository.Officers.Add(CreateOfficer(2, "officer2@example.com"));

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenOfficerExists_ShouldReturnOfficer()
    {
        _repository.Officers.Add(CreateOfficer(1, "officer@example.com"));

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("officer@example.com", result.Email);
        Assert.Equal("ABC College", result.CollegeName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenOfficerDoesNotExist_ShouldReturnNull()
    {
        var result = await _service.GetByIdAsync(100);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCollegeIdAsync_ShouldReturnOfficersOfSelectedCollege()
    {
        _repository.Officers.Add(CreateOfficer(1, "one@example.com", 1));
        _repository.Officers.Add(CreateOfficer(2, "two@example.com", 2));

        var result = await _service.GetByCollegeIdAsync(1);

        Assert.Single(result);
        Assert.Equal("one@example.com", result.First().Email);
    }

    [Fact]
    public async Task ApproveAsync_WhenOfficerDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.ApproveAsync(100);

        Assert.False(result);
    }

    [Fact]
    public async Task ApproveAsync_WhenOfficerIsPending_ShouldActivateUserAndAssignRole()
    {
        var officer = CreateOfficer(1, "officer@example.com");
        _repository.Officers.Add(officer);

        var result = await _service.ApproveAsync(1);

        Assert.True(result);
        Assert.Equal(AccountStatus.Active, officer.User.AccountStatus);
        Assert.Equal("PlacementOfficer", _userManager.AddedRole);
    }

    [Fact]
    public async Task ApproveAsync_WhenOfficerIsNotPending_ShouldThrowException()
    {
        var officer = CreateOfficer(1, "officer@example.com");
        officer.User.AccountStatus = AccountStatus.Active;

        _repository.Officers.Add(officer);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApproveAsync(1));
    }

    [Fact]
    public async Task RejectAsync_WhenOfficerDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.RejectAsync(100);

        Assert.False(result);
    }

    [Fact]
    public async Task RejectAsync_WhenOfficerIsPending_ShouldSetRejectedStatus()
    {
        var officer = CreateOfficer(1, "officer@example.com");
        _repository.Officers.Add(officer);

        var result = await _service.RejectAsync(1);

        Assert.True(result);
        Assert.Equal(AccountStatus.Rejected, officer.User.AccountStatus);
    }

    [Fact]
    public async Task RejectAsync_WhenOfficerIsNotPending_ShouldThrowException()
    {
        var officer = CreateOfficer(1, "officer@example.com");
        officer.User.AccountStatus = AccountStatus.Active;

        _repository.Officers.Add(officer);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.RejectAsync(1));
    }

    private static PlacementOfficer CreateOfficer(
        int id,
        string email,
        int collegeId = 1)
    {
        var college = new College
        {
            Id = collegeId,
            CollegeName = collegeId == 1
                ? "ABC College"
                : "XYZ College"
        };

        var user = new ApplicationUser
        {
            Id = $"user-{id}",
            UserName = email,
            Email = email,
            PhoneNumber = "9876543210",
            AccountStatus = AccountStatus.Pending
        };

        return new PlacementOfficer
        {
            Id = id,
            UserId = user.Id,
            User = user,
            CollegeId = collegeId,
            College = college,
            CreatedAt = DateTime.UtcNow
        };
    }

    private class FakePlacementOfficerRepository
        : IPlacementOfficerRepository
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

        public Task<IEnumerable<PlacementOfficer>> GetByCollegeIdAsync(
            int collegeId)
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

    private class FakeUserManager : UserManager<ApplicationUser>
    {
        public string? AddedRole { get; private set; }

        public FakeUserManager(IUserStore<ApplicationUser> store)
            : base(
                store,
               Microsoft.Extensions.Options.Options.Create(new IdentityOptions()),
                new PasswordHasher<ApplicationUser>(),
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null!,
                new Logger<FakeUserManager>(
                    new LoggerFactory()))
        {
        }

        public override Task<IdentityResult> AddToRoleAsync(
            ApplicationUser user,
            string role)
        {
            AddedRole = role;
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> UpdateAsync(
            ApplicationUser user)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
