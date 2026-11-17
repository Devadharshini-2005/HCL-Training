
using FoodHub.Models;

namespace FoodHub.Repositories;

public interface IFoodItemRepository
{
    Task<IEnumerable<FoodItem>> GetAllAsync();

    Task<FoodItem?> GetByIdAsync(int id);

    Task<FoodItem> AddAsync(FoodItem foodItem);

    Task UpdateAsync(FoodItem foodItem);

    Task DeleteAsync(FoodItem foodItem);
}
