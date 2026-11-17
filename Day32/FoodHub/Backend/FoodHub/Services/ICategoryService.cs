
using FoodHub.DTOs.Category.Requests;
using FoodHub.DTOs.Category.Responses;

namespace FoodHub.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto request);
}
