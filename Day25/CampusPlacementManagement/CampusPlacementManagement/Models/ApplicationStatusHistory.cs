using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models;

public class ApplicationStatusHistory
{
    public int Id { get; set; }

    public int JobApplicationId { get; set; }

    public JobApplicationStatus Status { get; set; }

    public string? Remarks { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties

    public JobApplication JobApplication { get; set; } = null!;

    public ApplicationUser ChangedByUser { get; set; } = null!;
}