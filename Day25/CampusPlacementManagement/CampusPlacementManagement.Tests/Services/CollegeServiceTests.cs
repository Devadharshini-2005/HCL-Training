
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;

namespace CampusPlacementManagement.Tests.Services;

public class CollegeServiceTests
{
    private readonly FakeCollegeRepository _repository;
    private readonly CollegeService _service;

    public CollegeServiceTests()
    {
        _repository = new FakeCollegeRepository();
        _service = new CollegeService(_repository);
    }

    // 1. Get all colleges
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllColleges()
    {
        _repository.Colleges.Add(CreateCollege(1, "ABC College"));
        _repository.Colleges.Add(CreateCollege(2, "XYZ College"));

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    // 2. Get college by valid ID
    [Fact]
    public async Task GetByIdAsync_WhenCollegeExists_ShouldReturnCollege()
    {
        _repository.Colleges.Add(CreateCollege(1, "ABC College"));

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("ABC College", result.CollegeName);
    }

    // 3. Get college by invalid ID
    [Fact]
    public async Task GetByIdAsync_WhenCollegeDoesNotExist_ShouldReturnNull()
    {
        var result = await _service.GetByIdAsync(100);

        Assert.Null(result);
    }

    // 4. Get pending colleges
    [Fact]
    public async Task GetPendingCollegesAsync_ShouldReturnOnlyPendingColleges()
    {
        _repository.Colleges.Add(CreateCollege(1, "ABC College"));
        _repository.Colleges.Add(CreateCollege(
            2, "XYZ College", CollegeStatus.Approved));

        var result = await _service.GetPendingCollegesAsync();

        Assert.Single(result);
        Assert.Equal(CollegeStatus.Pending, result.First().Status);
    }

    // 5. Create college successfully
    [Fact]
    public async Task CreateAsync_ShouldCreateCollegeWithPendingStatus()
    {
        var request = new CreateCollegeRequestDto
        {
            CollegeName = "ABC Engineering College",
            CollegeCode = "ABC001",
            Address = "Main Road",
            City = "Dindigul",
            State = "Tamil Nadu",
            Email = "abc@example.com",
            PhoneNumber = "9876543210"
        };

        var result = await _service.CreateAsync(request);

        Assert.Equal("ABC Engineering College", result.CollegeName);
        Assert.Equal("ABC001", result.CollegeCode);
        Assert.Equal(CollegeStatus.Pending, result.Status);
        Assert.Single(_repository.Colleges);
    }

    // 6. Duplicate college code
    [Fact]
    public async Task CreateAsync_WhenCollegeCodeExists_ShouldThrowException()
    {
        var existingCollege = CreateCollege(1, "ABC College");
        existingCollege.CollegeCode = "ABC001";

        _repository.Colleges.Add(existingCollege);

        var request = new CreateCollegeRequestDto
        {
            CollegeName = "Another College",
            CollegeCode = "ABC001",
            Address = "Main Road",
            City = "Dindigul",
            State = "Tamil Nadu",
            Email = "another@example.com",
            PhoneNumber = "9876543210"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(request));
    }

    // 7. Update college successfully
    [Fact]
    public async Task UpdateAsync_WhenCollegeExists_ShouldUpdateCollege()
    {
        _repository.Colleges.Add(CreateCollege(1, "Old College"));

        var request = new UpdateCollegeRequestDto
        {
            CollegeName = "Updated College",
            Address = "New Address",
            City = "Chennai",
            State = "Tamil Nadu",
            Email = "updated@example.com",
            PhoneNumber = "9999999999"
        };

        var result = await _service.UpdateAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal("Updated College", result.CollegeName);
        Assert.Equal("Chennai", result.City);
    }

    // 8. Update college with invalid ID
    [Fact]
    public async Task UpdateAsync_WhenCollegeDoesNotExist_ShouldReturnNull()
    {
        var request = new UpdateCollegeRequestDto
        {
            CollegeName = "Updated College",
            Address = "New Address",
            City = "Chennai",
            State = "Tamil Nadu",
            Email = "updated@example.com",
            PhoneNumber = "9999999999"
        };

        var result = await _service.UpdateAsync(100, request);

        Assert.Null(result);
    }

    // 9. Approve pending college
    [Fact]
    public async Task ApproveAsync_WhenCollegeIsPending_ShouldApproveCollege()
    {
        var college = CreateCollege(1, "ABC College");
        _repository.Colleges.Add(college);

        var result = await _service.ApproveAsync(1);

        Assert.True(result);
        Assert.Equal(CollegeStatus.Approved, college.Status);
    }

    // 10. Approve nonexistent college
    [Fact]
    public async Task ApproveAsync_WhenCollegeDoesNotExist_ShouldReturnFalse()
    {
        var result = await _service.ApproveAsync(100);

        Assert.False(result);
    }

    // 11. Approve already approved college
    [Fact]
    public async Task ApproveAsync_WhenCollegeIsAlreadyApproved_ShouldThrowException()
    {
        _repository.Colleges.Add(
            CreateCollege(1, "ABC College", CollegeStatus.Approved));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ApproveAsync(1));
    }

    // 12. Reject pending college
    [Fact]
    public async Task RejectAsync_WhenCollegeIsPending_ShouldRejectCollege()
    {
        var college = CreateCollege(1, "ABC College");
        _repository.Colleges.Add(college);

        var result = await _service.RejectAsync(1);

        Assert.True(result);
        Assert.Equal(CollegeStatus.Rejected, college.Status);
    }

    // 13. Reject already approved college
    [Fact]
    public async Task RejectAsync_WhenCollegeIsApproved_ShouldThrowException()
    {
        _repository.Colleges.Add(
            CreateCollege(1, "ABC College", CollegeStatus.Approved));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.RejectAsync(1));
    }

    // Helper method
    private static College CreateCollege(
        int id,
        string name,
        CollegeStatus status = CollegeStatus.Pending)
    {
        return new College
        {
            Id = id,
            CollegeName = name,
            CollegeCode = $"COL{id:000}",
            Address = "Main Road",
            City = "Dindigul",
            State = "Tamil Nadu",
            Email = $"college{id}@example.com",
            PhoneNumber = "9876543210",
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Fake repository for unit testing
    private class FakeCollegeRepository : ICollegeRepository
    {
        public List<College> Colleges { get; } = new();

        public Task<IEnumerable<College>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<College>>(Colleges);
        }

        public Task<College?> GetByIdAsync(int id)
        {
            return Task.FromResult(
                Colleges.FirstOrDefault(c => c.Id == id));
        }

        public Task<College?> GetByCollegeCodeAsync(string collegeCode)
        {
            return Task.FromResult(
                Colleges.FirstOrDefault(c => c.CollegeCode == collegeCode));
        }

        public Task<IEnumerable<College>> GetPendingCollegesAsync()
        {
            return Task.FromResult<IEnumerable<College>>(
                Colleges.Where(c => c.Status == CollegeStatus.Pending).ToList());
        }

        public Task AddAsync(College college)
        {
            college.Id = Colleges.Count == 0
                ? 1
                : Colleges.Max(c => c.Id) + 1;

            Colleges.Add(college);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(College college)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(Colleges.Any(c => c.Id == id));
        }

        public Task<bool> CollegeCodeExistsAsync(string collegeCode)
        {
            return Task.FromResult(
                Colleges.Any(c => c.CollegeCode == collegeCode));
        }
    }
}
