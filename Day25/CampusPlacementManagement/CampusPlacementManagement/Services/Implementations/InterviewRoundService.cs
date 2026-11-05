
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class InterviewRoundService : IInterviewRoundService
{
    private readonly IInterviewRoundRepository _interviewRoundRepository;
    private readonly ApplicationDbContext _context;

    public InterviewRoundService(
        IInterviewRoundRepository interviewRoundRepository,
        ApplicationDbContext context)
    {
        _interviewRoundRepository = interviewRoundRepository;
        _context = context;
    }

    public async Task<List<InterviewRoundResponseDto>> GetByJobOpeningIdAsync(
        int jobOpeningId,
        string userId,
        string userRole)
    {
        var job = await _context.JobOpenings
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == jobOpeningId);

        if (job == null)
        {
            throw new KeyNotFoundException("Job opening not found.");
        }

        if (userRole == "SuperAdmin")
        {
            // SuperAdmin can view all interview rounds.
        }
        else if (userRole == "CompanyHR")
        {
            var hr = await GetActiveCompanyHRAsync(userId);

            if (job.CompanyId != hr.CompanyId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot view another company's interview rounds.");
            }
        }
        else if (userRole == "Student")
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null ||
                student.User.AccountStatus != AccountStatus.Active ||
                student.College.Status != CollegeStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "Active student access is required.");
            }

            var hasApplied = await _context.JobApplications
                .AnyAsync(a =>
                    a.JobOpeningId == jobOpeningId &&
                    a.StudentId == student.Id);

            if (!hasApplied)
            {
                throw new UnauthorizedAccessException(
                    "You must apply for this job to view its interview rounds.");
            }
        }
        else if (userRole == "PlacementOfficer")
        {
            var officer = await _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (officer == null ||
                officer.User.AccountStatus != AccountStatus.Active ||
                officer.College.Status != CollegeStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "Active Placement Officer access is required.");
            }

            var isTargetCollege = await _context.JobTargetColleges
                .AnyAsync(t =>
                    t.JobOpeningId == jobOpeningId &&
                    t.CollegeId == officer.CollegeId);

            if (!isTargetCollege)
            {
                throw new UnauthorizedAccessException(
                    "This job is not targeted to your college.");
            }
        }
        else
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to view interview rounds.");
        }

        var rounds = await _interviewRoundRepository
            .GetByJobOpeningIdAsync(jobOpeningId);

        return rounds.Select(MapToResponse).ToList();
    }

    public async Task<InterviewRoundResponseDto> CreateAsync(
        int jobOpeningId,
        CreateInterviewRoundRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var job = await _context.JobOpenings
            .FirstOrDefaultAsync(j => j.Id == jobOpeningId);

        if (job == null)
        {
            throw new KeyNotFoundException("Job opening not found.");
        }

        if (job.CompanyId != hr.CompanyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot create interview rounds for another company's job.");
        }

        if (job.Status != JobStatus.Published)
        {
            throw new InvalidOperationException(
                "Interview rounds can only be created for published jobs.");
        }

        var existingRound = await _interviewRoundRepository
            .GetByJobAndRoundNumberAsync(
                jobOpeningId,
                request.RoundNumber);

        if (existingRound != null)
        {
            throw new InvalidOperationException(
                "This round number already exists for this job.");
        }

        var round = new InterviewRound
        {
            JobOpeningId = jobOpeningId,
            RoundName = request.RoundName,
            RoundNumber = request.RoundNumber,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _interviewRoundRepository.AddAsync(round);

        var createdRound = await _interviewRoundRepository
            .GetByIdAsync(round.Id);

        return MapToResponse(createdRound!);
    }

    public async Task<InterviewRoundResponseDto> UpdateAsync(
        int id,
        UpdateInterviewRoundRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var round = await _interviewRoundRepository.GetByIdAsync(id);

        if (round == null)
        {
            throw new KeyNotFoundException("Interview round not found.");
        }

        if (round.JobOpening.CompanyId != hr.CompanyId)
        {
            throw new UnauthorizedAccessException(
                "You cannot update another company's interview round.");
        }

        var existingRound = await _interviewRoundRepository
            .GetByJobAndRoundNumberAsync(
                round.JobOpeningId,
                request.RoundNumber);

        if (existingRound != null && existingRound.Id != id)
        {
            throw new InvalidOperationException(
                "This round number already exists for this job.");
        }

        round.RoundName = request.RoundName;
        round.RoundNumber = request.RoundNumber;
        round.Description = request.Description;

        await _interviewRoundRepository.UpdateAsync(round);

        return MapToResponse(round);
    }

    private async Task<CompanyHR> GetActiveCompanyHRAsync(string userId)
    {
        var hr = await _context.CompanyHRs
            .Include(h => h.User)
            .Include(h => h.Company)
            .FirstOrDefaultAsync(h => h.UserId == userId);

        if (hr == null ||
            hr.User.AccountStatus != AccountStatus.Active ||
            hr.Company.Status != CompanyStatus.Approved)
        {
            throw new UnauthorizedAccessException(
                "Active CompanyHR access is required.");
        }

        return hr;
    }

    private static InterviewRoundResponseDto MapToResponse(
        InterviewRound round)
    {
        return new InterviewRoundResponseDto
        {
            Id = round.Id,
            JobOpeningId = round.JobOpeningId,
            JobTitle = round.JobOpening.JobTitle,
            RoundName = round.RoundName,
            RoundNumber = round.RoundNumber,
            Description = round.Description,
            CreatedAt = round.CreatedAt
        };
    }
}
