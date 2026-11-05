
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class UpdateInterviewRoundRequestDto
{
    [Required]
    [StringLength(100)]
    public string RoundName { get; set; } = string.Empty;

    [Range(1, 20)]
    public int RoundNumber { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}
