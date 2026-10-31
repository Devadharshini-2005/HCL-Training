using System.ComponentModel.DataAnnotations;

namespace WebApiPractice.DTOs
{
    public class CreateStudentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Range(19, 30)]
        public int Age { get; set; }
    }
}
