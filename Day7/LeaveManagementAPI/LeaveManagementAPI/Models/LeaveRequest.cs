
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementAPI.Models
{
    public class LeaveRequest : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        public string LeaveType { get; set; } = string.Empty;

        [Required]
        public DateOnly? StartDate { get; set; }

        [Required]
        public DateOnly? EndDate { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            "^(Pending|Approved|Rejected)$",
            ErrorMessage = "Status must be Pending, Approved, or Rejected.")]
        public string Status { get; set; } = "Pending";

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue &&
                EndDate.Value < StartDate.Value)
            {
                yield return new ValidationResult(
                    "EndDate cannot be before StartDate.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}