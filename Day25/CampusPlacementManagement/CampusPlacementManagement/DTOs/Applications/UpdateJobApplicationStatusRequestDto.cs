
using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Applications;

public class UpdateJobApplicationStatusRequestDto
{
    [Required]
    [EnumDataType(typeof(JobApplicationStatus),
        ErrorMessage = "Select a valid application status.")]
    public JobApplicationStatus Status { get; set; }

    [StringLength(1000,
        ErrorMessage = "Remarks cannot exceed 1000 characters.")]
    public string? Remarks { get; set; }
}
