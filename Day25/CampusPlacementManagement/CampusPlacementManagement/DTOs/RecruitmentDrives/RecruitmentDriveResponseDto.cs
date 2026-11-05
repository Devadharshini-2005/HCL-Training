
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.RecruitmentDrives;

public class RecruitmentDriveResponseDto
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public int CollegeId { get; set; }

    public string CollegeName { get; set; } = string.Empty;

    public string DriveName { get; set; } = string.Empty;

    public DateTime DriveDate { get; set; }

    public string? Venue { get; set; }

    public string? Description { get; set; }

    public RecruitmentDriveStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
