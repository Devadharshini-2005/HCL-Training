
using System.ComponentModel.DataAnnotations;

namespace FoodHub.DTOs.FoodItem.Requests;

public class CreateFoodItemDto
{
    [Required(ErrorMessage = "Food name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Food name must be between 2 and 100 characters.")]
    public string FoodName { get; set; } = string.Empty;

    [StringLength(500,
        ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 100000,
        ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "Please provide a valid image URL.")]
    public string? ImageUrl { get; set; }

    [Required]
    [Range(1, int.MaxValue,
        ErrorMessage = "Please select a valid category.")]
    public int CategoryId { get; set; }
}
