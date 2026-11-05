namespace CampusPlacementManagement.Models;

public class JobEligibleGraduationYear
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public int GraduationYear { get; set; }

    // Navigation property

    public JobOpening JobOpening { get; set; } = null!;
}