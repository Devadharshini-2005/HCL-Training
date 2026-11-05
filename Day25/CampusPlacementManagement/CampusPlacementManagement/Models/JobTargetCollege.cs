namespace CampusPlacementManagement.Models;

public class JobTargetCollege
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public int CollegeId { get; set; }

    // Navigation properties

    public JobOpening JobOpening { get; set; } = null!;

    public College College { get; set; } = null!;
}