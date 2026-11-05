
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class UpdateInterviewScheduleRequestDto
{
    [Required]
    public DateTime ScheduledAt { get; set; }

    [StringLength(500)]
    public string? MeetingLink { get; set; }

    [StringLength(255)]
    public string? Venue { get; set; }
}
