using System.ComponentModel.DataAnnotations;

namespace WebApiPractice.DTOs;

public class UpdateStudentDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(17, 60)]
    public int Age { get; set; }
}

