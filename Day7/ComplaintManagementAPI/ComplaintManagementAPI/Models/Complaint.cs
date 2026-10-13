
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintManagementAPI.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string SubmittedBy { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Open|InProgress|Resolved)$")]
        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; }
    }
}