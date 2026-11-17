
using FoodHub.DTOs.FoodItem.Requests;
using FoodHub.DTOs.FoodItem.Responses;

namespace FoodHub.Services;

public interface IFoodItemService
{
    Task<IEnumerable<FoodItemResponseDto>> GetAllAsync();

    Task<FoodItemResponseDto?> GetByIdAsync(int id);

    Task<FoodItemResponseDto> AddAsync(CreateFoodItemDto foodItemDto);

    Task UpdateAsync(int id, UpdateFoodItemDto foodItemDto);

    Task DeleteAsync(int id);
}
