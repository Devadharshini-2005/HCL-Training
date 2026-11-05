namespace CampusPlacementManagement.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    // Store the hash, not the original refresh token
    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    // Navigation property
    public CampusPlacementManagement.Models.Identity.ApplicationUser User { get; set; } = null!;
}