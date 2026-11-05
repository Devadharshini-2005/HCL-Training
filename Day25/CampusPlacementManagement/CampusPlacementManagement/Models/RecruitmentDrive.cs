namespace CampusPlacementManagement.Models;

public class RecruitmentDrive
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public int CollegeId { get; set; }

    public string DriveName { get; set; } = string.Empty;

    public DateTime DriveDate { get; set; }

    public string Venue { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties

    public JobOpening JobOpening { get; set; } = null!;

    public College College { get; set; } = null!;
}