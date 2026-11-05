using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Applications;

public class ApplicationStatusHistoryResponseDto
{
    public int Id { get; set; }

    public JobApplicationStatus Status { get; set; }

    public string? Remarks { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; }
}