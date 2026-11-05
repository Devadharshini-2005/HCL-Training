
namespace CampusPlacementManagement.Models;

public class InterviewRound
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public string RoundName { get; set; } = string.Empty;

    public int RoundNumber { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public JobOpening JobOpening { get; set; } = null!;

    public ICollection<InterviewSchedule> InterviewSchedules { get; set; }
        = new List<InterviewSchedule>();
}
