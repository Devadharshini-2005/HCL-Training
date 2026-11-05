
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.RecruitmentDrives;

public class CreateRecruitmentDriveRequestDto
{
    [Required]
    public int JobOpeningId { get; set; }

    [Required]
    public int CollegeId { get; set; }

    [Required]
    [StringLength(200)]
    public string DriveName { get; set; } = string.Empty;

    [Required]
    public DateTime DriveDate { get; set; }

    [StringLength(255)]
    public string? Venue { get; set; }

    public string? Description { get; set; }
}
