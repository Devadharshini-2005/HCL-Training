
namespace CampusPlacementManagement.DTOs.CompanyHRs;

public class CompanyHRResponseDto
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string AccountStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
