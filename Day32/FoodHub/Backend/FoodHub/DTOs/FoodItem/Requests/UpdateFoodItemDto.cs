
using System.ComponentModel.DataAnnotations;

namespace FoodHub.DTOs.FoodItem.Requests;

public class UpdateFoodItemDto
{
    [Required(ErrorMessage = "Food name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string FoodName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "Please provide a valid image URL.")]
    public string? ImageUrl { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
