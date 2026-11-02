using System.ComponentModel.DataAnnotations;

namespace WebApiPractice.DTOs;

public class CreateDepartmentDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string DepartmentName { get; set; } = string.Empty;
}

