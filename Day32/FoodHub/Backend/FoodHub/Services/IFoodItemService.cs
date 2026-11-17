
using FoodHub.Models;

namespace FoodHub.Services;

public interface IFoodItemService
{
    Task<IEnumerable<FoodItem>> GetAllAsync();

    Task<FoodItem?> GetByIdAsync(int id);

    Task<FoodItem> AddAsync(FoodItem foodItem);

    Task UpdateAsync(int id, FoodItem foodItem);

    Task DeleteAsync(int id);
}
