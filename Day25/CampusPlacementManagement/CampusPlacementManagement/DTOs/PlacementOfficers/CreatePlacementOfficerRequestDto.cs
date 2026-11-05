
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.PlacementOfficers;

public class CreatePlacementOfficerRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int CollegeId { get; set; }
}
