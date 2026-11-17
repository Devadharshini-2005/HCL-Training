using FoodHub.DTOs.Category.Responses;
using FoodHub.Repositories;

namespace FoodHub.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(category => new CategoryResponseDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        });
    }
}