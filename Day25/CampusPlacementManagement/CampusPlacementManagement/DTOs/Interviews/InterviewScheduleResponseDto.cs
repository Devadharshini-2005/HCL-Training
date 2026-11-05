
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Interviews;

public class InterviewScheduleResponseDto
{
    public int Id { get; set; }

    public int JobApplicationId { get; set; }

    public string ApplicationReference { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public int InterviewRoundId { get; set; }

    public string RoundName { get; set; } = string.Empty;

    public int RoundNumber { get; set; }

    public DateTime ScheduledAt { get; set; }

    public string? MeetingLink { get; set; }

    public string? Venue { get; set; }

    public InterviewStatus Status { get; set; }

    public string? Feedback { get; set; }

    public DateTime CreatedAt { get; set; }
}
