
namespace FoodHub.Models;

public class FoodAvailability
{
    public int FoodAvailabilityId { get; set; }

    public int FoodItemId { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime LastUpdated { get; set; }

    // Navigation Property
    public FoodItem FoodItem { get; set; } = null!;
}
