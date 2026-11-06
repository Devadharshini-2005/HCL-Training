
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.RecruitmentDrives;

public class UpdateRecruitmentDriveRequestDto
{
    [Required(ErrorMessage = "Drive name is required.")]
    [StringLength(
        200,
        MinimumLength = 2,
        ErrorMessage = "Drive name must be between 2 and 200 characters.")]
    public string DriveName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Drive date is required.")]
    [CustomValidation(
        typeof(UpdateRecruitmentDriveRequestDto),
        nameof(ValidateDriveDate))]
    public DateTime DriveDate { get; set; }

    [StringLength(255, ErrorMessage = "Venue cannot exceed 255 characters.")]
    public string? Venue { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    public static ValidationResult? ValidateDriveDate(
        DateTime driveDate,
        ValidationContext context)
    {
        if (driveDate == default)
        {
            return new ValidationResult("Drive date is required.");
        }

        if (driveDate <= DateTime.UtcNow)
        {
            return new ValidationResult("Drive date must be in the future.");
        }

        return ValidationResult.Success;
    }
}
