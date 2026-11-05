
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations;

public class InterviewScheduleService : IInterviewScheduleService
{
    private readonly ApplicationDbContext _context;
    private readonly IInterviewScheduleRepository _interviewScheduleRepository;

    public InterviewScheduleService(
        ApplicationDbContext context,
        IInterviewScheduleRepository interviewScheduleRepository)
    {
        _context = context;
        _interviewScheduleRepository = interviewScheduleRepository;
    }

    // GET: View interviews for an application
    public async Task<List<InterviewScheduleResponseDto>> GetByApplicationIdAsync(
        int applicationId,
        string userId,
        string userRole)
    {
        var application = await _context.JobApplications
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Student)
                .ThenInclude(s => s.College)
            .Include(a => a.JobOpening)
                .ThenInclude(j => j.Company)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
            throw new KeyNotFoundException("Job application not found.");

        if (userRole == "SuperAdmin")
        {
            // SuperAdmin can view all interview schedules.
        }
        else if (userRole == "CompanyHR")
        {
            var hr = await GetActiveCompanyHRAsync(userId);

            if (application.JobOpening.CompanyId != hr.CompanyId)
                throw new UnauthorizedAccessException(
                    "You cannot access another company's interviews.");
        }
        else if (userRole == "Student")
        {
            if (application.Student.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You can only view your own interviews.");

            if (application.Student.User.AccountStatus != AccountStatus.Active ||
                application.Student.College.Status != CollegeStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "Your student account or college is not active.");
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

            if (application.Student.CollegeId != officer.CollegeId)
                throw new UnauthorizedAccessException(
                    "You cannot access another college's interviews.");
        }
        else
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to view interviews.");
        }

        var schedules =
            await _interviewScheduleRepository.GetByApplicationIdAsync(
                applicationId);

        return schedules.Select(MapToResponse).ToList();
    }

    // POST: Schedule an interview
    public async Task<InterviewScheduleResponseDto> CreateAsync(
        int applicationId,
        CreateInterviewScheduleRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var application = await _context.JobApplications
            .Include(a => a.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
            throw new KeyNotFoundException("Job application not found.");

        if (application.JobOpening.CompanyId != hr.CompanyId)
            throw new UnauthorizedAccessException(
                "You cannot schedule interviews for another company's job.");

        var round = await _context.InterviewRounds
            .FirstOrDefaultAsync(r => r.Id == request.InterviewRoundId);

        if (round == null)
            throw new KeyNotFoundException("Interview round not found.");

        if (round.JobOpeningId != application.JobOpeningId)
            throw new InvalidOperationException(
                "The interview round does not belong to this job.");

        if (request.ScheduledAt <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "Interview date must be in the future.");

        var alreadyScheduled = await _context.InterviewSchedules
            .AnyAsync(i =>
                i.JobApplicationId == applicationId &&
                i.InterviewRoundId == request.InterviewRoundId);

        if (alreadyScheduled)
            throw new InvalidOperationException(
                "An interview is already scheduled for this application and round.");

        var interviewSchedule = new InterviewSchedule
        {
            JobApplicationId = applicationId,
            InterviewRoundId = request.InterviewRoundId,
            ScheduledAt = request.ScheduledAt,
            MeetingLink = request.MeetingLink,
            Venue = request.Venue,
            Status = InterviewStatus.Scheduled
        };

        await _interviewScheduleRepository.AddAsync(interviewSchedule);

        var savedSchedule =
            await _interviewScheduleRepository.GetByIdAsync(
                interviewSchedule.Id);

        return MapToResponse(savedSchedule!);
    }

    // PUT: Reschedule an interview
    public async Task<InterviewScheduleResponseDto> UpdateAsync(
        int id,
        UpdateInterviewScheduleRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var interview = await _interviewScheduleRepository.GetByIdAsync(id);

        if (interview == null)
            throw new KeyNotFoundException("Interview schedule not found.");

        if (interview.JobApplication.JobOpening.CompanyId != hr.CompanyId)
            throw new UnauthorizedAccessException(
                "You cannot update another company's interview.");

        if (interview.Status == InterviewStatus.Completed ||
            interview.Status == InterviewStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Completed or cancelled interviews cannot be rescheduled.");
        }

        if (request.ScheduledAt <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "Interview date must be in the future.");

        interview.ScheduledAt = request.ScheduledAt;
        interview.MeetingLink = request.MeetingLink;
        interview.Venue = request.Venue;
        interview.Status = InterviewStatus.Rescheduled;

        await _interviewScheduleRepository.UpdateAsync(interview);

        return MapToResponse(interview);
    }

    // PUT: Update interview feedback
    public async Task<InterviewScheduleResponseDto> UpdateFeedbackAsync(
        int id,
        UpdateInterviewFeedbackRequestDto request,
        string companyHRUserId)
    {
        var hr = await GetActiveCompanyHRAsync(companyHRUserId);

        var interview = await _interviewScheduleRepository.GetByIdAsync(id);

        if (interview == null)
            throw new KeyNotFoundException("Interview schedule not found.");

        if (interview.JobApplication.JobOpening.CompanyId != hr.CompanyId)
            throw new UnauthorizedAccessException(
                "You cannot update another company's interview feedback.");

        interview.Feedback = request.Feedback;

        await _interviewScheduleRepository.UpdateAsync(interview);

        return MapToResponse(interview);
    }

    // Check Company HR account and company status
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
                "Active Company HR access is required.");
        }

        return hr;
    }

    // Convert entity into response DTO
    private static InterviewScheduleResponseDto MapToResponse(
        InterviewSchedule interview)
    {
        return new InterviewScheduleResponseDto
        {
            Id = interview.Id,
            JobApplicationId = interview.JobApplicationId,
            ApplicationReference =
                interview.JobApplication.ApplicationReference,
            StudentName =
                interview.JobApplication.Student.User.UserName ?? string.Empty,
            InterviewRoundId = interview.InterviewRoundId,
            RoundName = interview.InterviewRound.RoundName,
            RoundNumber = interview.InterviewRound.RoundNumber,
            ScheduledAt = interview.ScheduledAt,
            MeetingLink = interview.MeetingLink,
            Venue = interview.Venue,
            Status = interview.Status,
            Feedback = interview.Feedback,
            CreatedAt = interview.CreatedAt
        };
    }
}
