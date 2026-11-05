
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class UpdateInterviewFeedbackRequestDto
{
    [Required]
    [StringLength(2000)]
    public string Feedback { get; set; } = string.Empty;
}
