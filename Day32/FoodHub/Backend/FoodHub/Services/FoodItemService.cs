
using FoodHub.DTOs.FoodItem.Requests;
using FoodHub.DTOs.FoodItem.Responses;
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
    public async Task<IEnumerable<FoodItemResponseDto>> GetAllAsync()
    {
        var foodItems = await _foodItemRepository.GetAllAsync();

        return foodItems.Select(MapToResponseDto);
    }

    // Get food item by ID
    public async Task<FoodItemResponseDto?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Food item ID must be greater than zero.");
        }

        var foodItem = await _foodItemRepository.GetByIdAsync(id);

        return foodItem == null ? null : MapToResponseDto(foodItem);
    }

    // Add food item
    public async Task<FoodItemResponseDto> AddAsync(
        CreateFoodItemDto foodItemDto)
    {
        ValidateFoodItem(
            foodItemDto.FoodName,
            foodItemDto.Price);

        var foodItem = new FoodItem
        {
            FoodName = foodItemDto.FoodName,
            Description = foodItemDto.Description,
            Price = foodItemDto.Price,
            ImageUrl = foodItemDto.ImageUrl,
            CategoryId = foodItemDto.CategoryId
        };

        var createdFoodItem =
            await _foodItemRepository.AddAsync(foodItem);

        var savedFoodItem =
            await _foodItemRepository.GetByIdAsync(
                createdFoodItem.FoodItemId);

        if (savedFoodItem == null)
        {
            throw new InvalidOperationException(
                "Unable to retrieve the created food item.");
        }

        return MapToResponseDto(savedFoodItem);
    }

    // Update food item
    public async Task UpdateAsync(
        int id,
        UpdateFoodItemDto foodItemDto)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Food item ID must be greater than zero.");
        }

        ValidateFoodItem(
            foodItemDto.FoodName,
            foodItemDto.Price);

        var existingFoodItem =
            await _foodItemRepository.GetByIdAsync(id);

        if (existingFoodItem == null)
        {
            throw new KeyNotFoundException(
                "Food item not found.");
        }

        existingFoodItem.FoodName = foodItemDto.FoodName;
        existingFoodItem.Description = foodItemDto.Description;
        existingFoodItem.Price = foodItemDto.Price;
        existingFoodItem.ImageUrl = foodItemDto.ImageUrl;
        existingFoodItem.CategoryId = foodItemDto.CategoryId;

        await _foodItemRepository.UpdateAsync(existingFoodItem);
    }

    // Delete food item
    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Food item ID must be greater than zero.");
        }

        var existingFoodItem =
            await _foodItemRepository.GetByIdAsync(id);

        if (existingFoodItem == null)
        {
            throw new KeyNotFoundException(
                "Food item not found.");
        }

        await _foodItemRepository.DeleteAsync(existingFoodItem);
    }

    // Basic business validation
    private static void ValidateFoodItem(
        string foodName,
        decimal price)
    {
        if (string.IsNullOrWhiteSpace(foodName))
        {
            throw new ArgumentException(
                "Food name is required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException(
                "Food price must be greater than zero.");
        }
    }

    // Convert Entity to Response DTO
    private static FoodItemResponseDto MapToResponseDto(
        FoodItem foodItem)
    {
        return new FoodItemResponseDto
        {
            FoodItemId = foodItem.FoodItemId,
            FoodName = foodItem.FoodName,
            Description = foodItem.Description,
            Price = foodItem.Price,
            ImageUrl = foodItem.ImageUrl,
            CategoryId = foodItem.CategoryId,
            CategoryName = foodItem.Category.CategoryName
        };
    }

    // Search food items
    public async Task<IEnumerable<FoodItemResponseDto>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("Search term cannot be empty.");
        }

        var foodItems = await _foodItemRepository.SearchAsync(searchTerm);

        return foodItems.Select(MapToResponseDto);
    }

}
