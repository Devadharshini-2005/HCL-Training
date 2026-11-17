
using System.ComponentModel.DataAnnotations;

namespace FoodHub.DTOs.Category.Requests;

public class UpdateCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}
