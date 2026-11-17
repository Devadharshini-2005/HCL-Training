using FoodHub.DTOs.Category.Responses;

namespace FoodHub.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
}