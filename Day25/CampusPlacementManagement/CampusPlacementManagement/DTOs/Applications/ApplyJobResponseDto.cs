using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Applications;

public class ApplyJobResponseDto
{
    public int Id { get; set; }

    public string ApplicationReference { get; set; } = string.Empty;

    public int JobOpeningId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string StudentEmail { get; set; } = string.Empty;

    public string RegisterNumber { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public JobApplicationStatus Status { get; set; }

    public DateTime AppliedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}