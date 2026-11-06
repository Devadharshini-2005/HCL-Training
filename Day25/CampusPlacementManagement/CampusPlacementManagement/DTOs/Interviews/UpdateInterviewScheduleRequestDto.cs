
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class UpdateInterviewScheduleRequestDto
{
    [Required(ErrorMessage = "Interview schedule date and time are required.")]
    [CustomValidation(
        typeof(UpdateInterviewScheduleRequestDto),
        nameof(ValidateScheduledAt))]
    public DateTime ScheduledAt { get; set; }

    [StringLength(500, ErrorMessage = "Meeting link cannot exceed 500 characters.")]
    [Url(ErrorMessage = "Enter a valid meeting URL.")]
    public string? MeetingLink { get; set; }

    [StringLength(255, ErrorMessage = "Venue cannot exceed 255 characters.")]
    public string? Venue { get; set; }

    public static ValidationResult? ValidateScheduledAt(
        DateTime scheduledAt,
        ValidationContext context)
    {
        if (scheduledAt == default)
        {
            return new ValidationResult("Scheduled date and time are required.");
        }

        if (scheduledAt <= DateTime.UtcNow)
        {
            return new ValidationResult(
                "Interview must be scheduled for a future date and time.");
        }

        return ValidationResult.Success;
    }
}
