
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class InterviewSchedule
{
    public int Id { get; set; }

    public int JobApplicationId { get; set; }

    public int InterviewRoundId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public string? MeetingLink { get; set; }

    public string? Venue { get; set; }

    public InterviewStatus Status { get; set; } = InterviewStatus.Scheduled;

    public string? Feedback { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public JobApplication JobApplication { get; set; } = null!;

    public InterviewRound InterviewRound { get; set; } = null!;
}
