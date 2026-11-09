
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;

namespace CampusPlacementManagement.Tests.Services;

public class CompanyServiceTests
{
    private readonly FakeCompanyRepository _repository;
    private readonly CompanyService _service;

    public CompanyServiceTests()
    {
        _repository = new FakeCompanyRepository();
        _service = new CompanyService(_repository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCompanies()
    {
        _repository.Companies.Add(CreateCompany(1, "ABC Technologies"));
        _repository.Companies.Add(CreateCompany(2, "XYZ Solutions"));

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyExists_ShouldReturnCompany()
    {
        _repository.Companies.Add(CreateCompany(1, "ABC Technologies"));

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("ABC Technologies", result.CompanyName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyDoesNotExist_ShouldReturnNull()
    {
        var result = await _service.GetByIdAsync(100);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPendingAsync_ShouldReturnOnlyPendingCompanies()
    {
        _repository.Companies.Add(CreateCompany(1, "ABC Technologies"));
        _repository.Companies.Add(CreateCompany(2, "XYZ Solutions"));

        _repository.Companies[1].Status = CompanyStatus.Approved;

        var result = await _service.GetPendingAsync();

        Assert.Single(result);
        Assert.Equal("ABC Technologies", result.First().CompanyName);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateCompanyWithPendingStatus()
    {
        var request = CreateCompanyRequest("ABC Technologies", "abc@example.com");

        var result = await _service.RegisterAsync(request);

        Assert.Equal("ABC Technologies", result.CompanyName);
        Assert.Equal("abc@example.com", result.Email);
        Assert.Equal(CompanyStatus.Pending, result.Status);
        Assert.Single(_repository.Companies);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldThrowException()
    {
        _repository.Companies.Add(CreateCompany(1, "ABC Technologies"));

        var request = CreateCompanyRequest(
            "New Company",
            "abc@example.com");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.RegisterAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_WhenCompanyExists_ShouldUpdateCompany()
    {
        _repository.Companies.Add(CreateCompany(1, "Old Company"));

        var request = CreateCompanyRequest(
            "Updated Company",
            "updated@example.com");

        var result = await _service.UpdateAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal("Updated Company", result.CompanyName);
        Assert.Equal("updated@example.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_WhenCompanyDoesNotExist_ShouldReturnNull()
    {
        var request = CreateCompanyRequest(
            "New Company",
            "new@example.com");

        var result = await _service.UpdateAsync(100, request);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenEmailBelongsToAnotherCompany_ShouldThrowException()
    {
        var firstCompany = CreateCompany(1, "ABC Technologies");
        firstCompany.Email = "abc@example.com";

        var secondCompany = CreateCompany(2, "XYZ Solutions");
        secondCompany.Email = "xyz@example.com";

        _repository.Companies.Add(firstCompany);
        _repository.Companies.Add(secondCompany);

        var request = CreateCompanyRequest(
            "Updated ABC",
            "xyz@example.com");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request));
    }


    [Fact]
    public async Task UpdateAsync_WhenEmailIsUnchanged_ShouldUpdateCompany()
    {
        _repository.Companies.Add(CreateCompany(1, "ABC Technologies"));

        var request = CreateCompanyRequest(
            "Updated ABC Technologies",
            "abc@example.com");

        var result = await _service.UpdateAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal("Updated ABC Technologies", result.CompanyName);
    }

    [Fact]
    public async Task ApproveAsync_WhenCompanyIsPending_ShouldApproveCompany()
    {
        var company = CreateCompany(1, "ABC Technologies");
        _repository.Companies.Add(company);

        var result = await _service.ApproveAsync(1);

        Assert.True(result);
        Assert.Equal(CompanyStatus.Approved, company.Status);
    }

    [Fact]
    public async Task ApproveAsync_WhenCompanyDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.ApproveAsync(100);

        Assert.False(result);
    }

    [Fact]
    public async Task ApproveAsync_WhenCompanyIsNotPending_ShouldThrowException()
    {
        var company = CreateCompany(1, "ABC Technologies");
        company.Status = CompanyStatus.Approved;
        _repository.Companies.Add(company);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApproveAsync(1));
    }

    [Fact]
    public async Task RejectAsync_WhenCompanyIsPending_ShouldRejectCompany()
    {
        var company = CreateCompany(1, "ABC Technologies");
        _repository.Companies.Add(company);

        var result = await _service.RejectAsync(1);

        Assert.True(result);
        Assert.Equal(CompanyStatus.Rejected, company.Status);
    }

    [Fact]
    public async Task RejectAsync_WhenCompanyDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.RejectAsync(100);

        Assert.False(result);
    }

    [Fact]
    public async Task RejectAsync_WhenCompanyIsNotPending_ShouldThrowException()
    {
        var company = CreateCompany(1, "ABC Technologies");
        company.Status = CompanyStatus.Approved;
        _repository.Companies.Add(company);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.RejectAsync(1));
    }

    private static Company CreateCompany(int id, string name)
    {
        return new Company
        {
            Id = id,
            CompanyName = name,
            Description = "Software development company",
            Website = "https://example.com",
            Address = "Chennai",
            City = "Chennai",
            State = "Tamil Nadu",
            Email = "abc@example.com",
            PhoneNumber = "9876543210",
            Status = CompanyStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static CreateCompanyRequestDto CreateCompanyRequest(
        string companyName,
        string email)
    {
        return new CreateCompanyRequestDto
        {
            CompanyName = companyName,
            Description = "Software development company",
            Website = "https://example.com",
            Address = "Chennai",
            City = "Chennai",
            State = "Tamil Nadu",
            Email = email,
            PhoneNumber = "9876543210"
        };
    }

    private class FakeCompanyRepository : ICompanyRepository
    {
        public List<Company> Companies { get; } = new();

        public Task<IEnumerable<Company>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Company>>(Companies);
        }

        public Task<Company?> GetByIdAsync(int id)
        {
            return Task.FromResult(
                Companies.FirstOrDefault(c => c.Id == id));
        }

        public Task<IEnumerable<Company>> GetPendingAsync()
        {
            return Task.FromResult<IEnumerable<Company>>(
                Companies.Where(c => c.Status == CompanyStatus.Pending)
                    .ToList());
        }

        public Task AddAsync(Company company)
        {
            company.Id = Companies.Count == 0
                ? 1
                : Companies.Max(c => c.Id) + 1;

            Companies.Add(company);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Company company)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(
                Companies.Any(c => c.Id == id));
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return Task.FromResult(
                Companies.Any(c => c.Email == email));
        }
    }
}
