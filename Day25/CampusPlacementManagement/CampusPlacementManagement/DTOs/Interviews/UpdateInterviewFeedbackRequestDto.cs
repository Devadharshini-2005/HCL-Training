
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class UpdateInterviewFeedbackRequestDto
{
    [Required(ErrorMessage = "Interview feedback is required.")]
    [StringLength(
        2000,
        MinimumLength = 2,
        ErrorMessage = "Feedback must be between 2 and 2000 characters.")]
    public string Feedback { get; set; } = string.Empty;
}
