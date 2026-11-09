
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CampusPlacementManagement.Tests.Services;

public class CompanyHRServiceTests
{
    private readonly FakeCompanyHRRepository _repository;
    private readonly FakeUserManager _userManager;
    private readonly CompanyHRService _service;

    public CompanyHRServiceTests()
    {
        _repository = new FakeCompanyHRRepository();
        _userManager = new FakeUserManager();

        _service = new CompanyHRService(
            _repository,
            null!,
            _userManager);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCompanyHRs()
    {
        _repository.CompanyHRs.Add(CreateCompanyHR(1, "hr1@example.com"));
        _repository.CompanyHRs.Add(CreateCompanyHR(2, "hr2@example.com"));

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenHRExists_ShouldReturnHR()
    {
        _repository.CompanyHRs.Add(CreateCompanyHR(1, "hr@example.com"));

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("hr@example.com", result.Email);
        Assert.Equal("ABC Technologies", result.CompanyName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenHRDoesNotExist_ShouldReturnNull()
    {
        var result = await _service.GetByIdAsync(100);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCompanyIdAsync_ShouldReturnHRsOfSelectedCompany()
    {
        _repository.CompanyHRs.Add(
            CreateCompanyHR(1, "hr1@example.com", 1));

        _repository.CompanyHRs.Add(
            CreateCompanyHR(2, "hr2@example.com", 2));

        var result = await _service.GetByCompanyIdAsync(1);

        Assert.Single(result);
        Assert.Equal("hr1@example.com", result.First().Email);
    }

    [Fact]
    public async Task ApproveAsync_WhenHRDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.ApproveAsync(100);

        Assert.False(result);
    }

    [Fact]
    public async Task ApproveAsync_WhenHRIsPending_ShouldActivateUserAndAssignRole()
    {
        var hr = CreateCompanyHR(1, "hr@example.com");
        _repository.CompanyHRs.Add(hr);

        var result = await _service.ApproveAsync(1);

        Assert.True(result);
        Assert.Equal(AccountStatus.Active, hr.User.AccountStatus);
        Assert.Equal("CompanyHR", _userManager.AddedRole);
    }

    [Fact]
    public async Task ApproveAsync_WhenHRIsNotPending_ShouldThrowException()
    {
        var hr = CreateCompanyHR(1, "hr@example.com");
        hr.User.AccountStatus = AccountStatus.Active;

        _repository.CompanyHRs.Add(hr);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApproveAsync(1));
    }

    [Fact]
    public async Task RejectAsync_WhenHRDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.RejectAsync(100);

        Assert.False(result);
    }

    [Fact]
    public async Task RejectAsync_WhenHRIsPending_ShouldSetRejectedStatus()
    {
        var hr = CreateCompanyHR(1, "hr@example.com");
        _repository.CompanyHRs.Add(hr);

        var result = await _service.RejectAsync(1);

        Assert.True(result);
        Assert.Equal(AccountStatus.Rejected, hr.User.AccountStatus);
    }

    [Fact]
    public async Task RejectAsync_WhenHRIsNotPending_ShouldThrowException()
    {
        var hr = CreateCompanyHR(1, "hr@example.com");
        hr.User.AccountStatus = AccountStatus.Active;

        _repository.CompanyHRs.Add(hr);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.RejectAsync(1));
    }

    private static CompanyHR CreateCompanyHR(
        int id,
        string email,
        int companyId = 1)
    {
        var company = new Company
        {
            Id = companyId,
            CompanyName = companyId == 1
                ? "ABC Technologies"
                : "XYZ Solutions"
        };

        var user = new ApplicationUser
        {
            Id = $"user-{id}",
            UserName = email,
            Email = email,
            PhoneNumber = "9876543210",
            AccountStatus = AccountStatus.Pending
        };

        return new CompanyHR
        {
            Id = id,
            UserId = user.Id,
            User = user,
            CompanyId = companyId,
            Company = company,
            CreatedAt = DateTime.UtcNow
        };
    }

    private class FakeCompanyHRRepository : ICompanyHRRepository
    {
        public List<CompanyHR> CompanyHRs { get; } = new();

        public Task<IEnumerable<CompanyHR>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CompanyHR>>(CompanyHRs);
        }

        public Task<CompanyHR?> GetByIdAsync(int id)
        {
            return Task.FromResult(
                CompanyHRs.FirstOrDefault(hr => hr.Id == id));
        }

        public Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(
            int companyId)
        {
            return Task.FromResult<IEnumerable<CompanyHR>>(
                CompanyHRs
                    .Where(hr => hr.CompanyId == companyId)
                    .ToList());
        }

        public Task AddAsync(CompanyHR companyHR)
        {
            CompanyHRs.Add(companyHR);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CompanyHR companyHR)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(
                CompanyHRs.Any(hr => hr.Id == id));
        }

        public Task<CompanyHR?> GetByUserIdAsync(string userId)
        {
            return Task.FromResult(
                CompanyHRs.FirstOrDefault(hr => hr.UserId == userId));
        }
    }

    private class FakeUserManager : UserManager<ApplicationUser>
    {
        public string? AddedRole { get; private set; }

        public FakeUserManager()
    : base(
        new FakeUserStore(),
        Microsoft.Extensions.Options.Options.Create(
            new IdentityOptions()),
        new PasswordHasher<ApplicationUser>(),
        Array.Empty<IUserValidator<ApplicationUser>>(),
        Array.Empty<IPasswordValidator<ApplicationUser>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        null!,
        new Logger<FakeUserManager>(new LoggerFactory()))
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

        private class FakeUserStore : IUserStore<ApplicationUser>
        {
            public Task<IdentityResult> CreateAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public Task<IdentityResult> UpdateAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public Task<IdentityResult> DeleteAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public Task<ApplicationUser?> FindByIdAsync(
                string userId,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<ApplicationUser?>(null);
            }

            public Task<ApplicationUser?> FindByNameAsync(
                string normalizedUserName,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<ApplicationUser?>(null);
            }

            public Task<string> GetUserIdAsync(
    ApplicationUser user,
    CancellationToken cancellationToken)
            {
                return Task.FromResult(user.Id);
            }

            public Task<string?> GetUserNameAsync(
                ApplicationUser user,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<string?>(user.UserName);
            }

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
            {
                return Task.FromResult<string?>(user.NormalizedUserName);
            }

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
