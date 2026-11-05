using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models;

public class PlacementOfficer
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CollegeId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties

    public ApplicationUser User { get; set; } = null!;

    public College College { get; set; } = null!;
}