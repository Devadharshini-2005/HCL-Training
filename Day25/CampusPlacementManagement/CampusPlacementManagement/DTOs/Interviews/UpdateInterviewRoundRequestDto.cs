
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class UpdateInterviewRoundRequestDto
{
    [Required(ErrorMessage = "Round name is required.")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Round name must be between 2 and 100 characters.")]
    public string RoundName { get; set; } = string.Empty;

    [Range(
        1,
        20,
        ErrorMessage = "Round number must be between 1 and 20.")]
    public int RoundNumber { get; set; }

    [StringLength(
        500,
        ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}
