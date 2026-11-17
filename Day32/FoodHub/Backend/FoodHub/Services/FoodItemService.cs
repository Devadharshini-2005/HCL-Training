
using FoodHub.Models;
using FoodHub.Repositories;

namespace FoodHub.Services;

public class FoodItemService : IFoodItemService
{
    private readonly IFoodItemRepository _foodItemRepository;

    public FoodItemService(IFoodItemRepository foodItemRepository)
    {
        _foodItemRepository = foodItemRepository;
    }

    // Get all food items
    public async Task<IEnumerable<FoodItem>> GetAllAsync()
    {
        return await _foodItemRepository.GetAllAsync();
    }

    // Get food item by ID
    public async Task<FoodItem?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Food item ID must be greater than zero.");
        }

        return await _foodItemRepository.GetByIdAsync(id);
    }

    // Add food item
    public async Task<FoodItem> AddAsync(FoodItem foodItem)
    {
        if (string.IsNullOrWhiteSpace(foodItem.FoodName))
        {
            throw new ArgumentException("Food name is required.");
        }

        if (foodItem.Price <= 0)
        {
            throw new ArgumentException("Food price must be greater than zero.");
        }

        return await _foodItemRepository.AddAsync(foodItem);
    }

    // Update food item
    public async Task UpdateAsync(int id, FoodItem foodItem)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Food item ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(foodItem.FoodName))
        {
            throw new ArgumentException("Food name is required.");
        }

        if (foodItem.Price <= 0)
        {
            throw new ArgumentException("Food price must be greater than zero.");
        }

        var existingFoodItem = await _foodItemRepository.GetByIdAsync(id);

        if (existingFoodItem == null)
        {
            throw new KeyNotFoundException("Food item not found.");
        }

        existingFoodItem.FoodName = foodItem.FoodName;
        existingFoodItem.Description = foodItem.Description;
        existingFoodItem.Price = foodItem.Price;
        existingFoodItem.ImageUrl = foodItem.ImageUrl;
        existingFoodItem.CategoryId = foodItem.CategoryId;

        await _foodItemRepository.UpdateAsync(existingFoodItem);
    }

    // Delete food item
    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Food item ID must be greater than zero.");
        }

        var existingFoodItem = await _foodItemRepository.GetByIdAsync(id);

        if (existingFoodItem == null)
        {
            throw new KeyNotFoundException("Food item not found.");
        }

        await _foodItemRepository.DeleteAsync(existingFoodItem);
    }
}
