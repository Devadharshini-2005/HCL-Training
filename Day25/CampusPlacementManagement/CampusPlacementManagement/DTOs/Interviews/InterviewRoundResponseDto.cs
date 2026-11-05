
namespace CampusPlacementManagement.DTOs.Interviews;

public class InterviewRoundResponseDto
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string RoundName { get; set; } = string.Empty;

    public int RoundNumber { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}
