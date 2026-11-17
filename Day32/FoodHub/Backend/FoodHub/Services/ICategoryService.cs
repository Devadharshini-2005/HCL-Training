
using FoodHub.DTOs.Category.Responses;

namespace FoodHub.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

    Task<CategoryResponseDto> CreateAsync(
        FoodHub.DTOs.Category.Requests.CreateCategoryDto request);

    Task<CategoryResponseDto?> GetByIdAsync(int id);
}
