
using FoodHub.DTOs.Category.Requests;
using FoodHub.DTOs.Category.Responses;
using FoodHub.Models;
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

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto request)
    {
        var category = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description
        };

        var createdCategory = await _categoryRepository.CreateAsync(category);

        return new CategoryResponseDto
        {
            CategoryId = createdCategory.CategoryId,
            CategoryName = createdCategory.CategoryName,
            Description = createdCategory.Description
        };
    }


    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return null;
        }

        return new CategoryResponseDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };
    }


}
