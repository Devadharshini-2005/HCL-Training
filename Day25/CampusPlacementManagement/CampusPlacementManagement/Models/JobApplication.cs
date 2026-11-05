using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class JobApplication
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int JobOpeningId { get; set; }

    public string ApplicationReference { get; set; } = string.Empty;

    public JobApplicationStatus Status { get; set; }
        = JobApplicationStatus.Applied;

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties

    public Student Student { get; set; } = null!;

    public JobOpening JobOpening { get; set; } = null!;
}