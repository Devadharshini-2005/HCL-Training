namespace CampusPlacementManagement.Models;

public class InterviewRound
{
    public int Id { get; set; }

    public int RecruitmentDriveId { get; set; }

    public string RoundName { get; set; } = string.Empty;

    public int RoundNumber { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property

    public RecruitmentDrive RecruitmentDrive { get; set; } = null!;
}