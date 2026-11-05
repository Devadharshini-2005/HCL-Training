using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Applications;

public class UpdateJobApplicationStatusRequestDto
{
    [Required]
    public JobApplicationStatus Status { get; set; }

    [StringLength(1000)]
    public string? Remarks { get; set; }
}