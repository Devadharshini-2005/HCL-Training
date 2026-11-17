
using FoodHub.DTOs.Category.Responses;
using FoodHub.DTOs.Category.Requests;
namespace FoodHub.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

    Task<CategoryResponseDto> CreateAsync(
        FoodHub.DTOs.Category.Requests.CreateCategoryDto request);

    Task<CategoryResponseDto?> GetByIdAsync(int id);

    Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto request);
    Task<bool> DeleteAsync(int id);

}
