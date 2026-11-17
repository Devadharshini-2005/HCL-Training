
namespace FoodHub.Models;

public class FoodItem
{
    public int FoodItemId { get; set; }

    public string FoodName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    // Foreign Key
    public int CategoryId { get; set; }

    // Navigation Property
    public Category Category { get; set; } = null!;
}
