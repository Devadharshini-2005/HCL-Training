using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models;

public class CompanyHR
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CompanyId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties

    public ApplicationUser User { get; set; } = null!;

    public Company Company { get; set; } = null!;
}