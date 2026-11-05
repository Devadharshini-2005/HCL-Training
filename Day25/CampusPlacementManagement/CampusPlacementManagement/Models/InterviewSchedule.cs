using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class InterviewSchedule
{
    public int Id { get; set; }

    public int JobApplicationId { get; set; }

    public int InterviewRoundId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public string? MeetingLink { get; set; }

    public string? Location { get; set; }

    public InterviewStatus Status { get; set; }
        = InterviewStatus.Scheduled;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties

    public JobApplication JobApplication { get; set; } = null!;

    public InterviewRound InterviewRound { get; set; } = null!;
}