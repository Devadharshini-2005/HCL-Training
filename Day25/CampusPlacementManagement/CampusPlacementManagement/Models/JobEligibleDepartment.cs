namespace CampusPlacementManagement.Models;

public class JobEligibleDepartment
{
    public int Id { get; set; }

    public int JobOpeningId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    // Navigation property

    public JobOpening JobOpening { get; set; } = null!;
}