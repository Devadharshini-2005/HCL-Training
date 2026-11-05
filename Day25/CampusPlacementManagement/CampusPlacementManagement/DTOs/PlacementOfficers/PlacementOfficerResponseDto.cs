
namespace CampusPlacementManagement.DTOs.PlacementOfficers;

public class PlacementOfficerResponseDto
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int CollegeId { get; set; }

    public string CollegeName { get; set; } = string.Empty;

    public string AccountStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
