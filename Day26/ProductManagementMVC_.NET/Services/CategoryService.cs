using ProductManagementMVC.Models;
using ProductManagementMVC.Repositories.Interfaces;
using ProductManagementMVC.Services.Interfaces;

namespace ProductManagementMVC.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _categoryRepository.GetAllAsync();
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        return _categoryRepository.GetByIdAsync(id);
    }

    public async Task<bool> CreateAsync(Category category)
    {
        if (await _categoryRepository.ExistsByNameAsync(category.CategoryName))
        {
            return false;
        }

        await _categoryRepository.AddAsync(category);
        return true;
    }

    public async Task<bool> UpdateAsync(int id, Category category)
    {
        Category? existing = await _categoryRepository.GetByIdAsync(id);

        if (existing is null ||
            await _categoryRepository.ExistsByNameAsync(category.CategoryName, id))
        {
            return false;
        }

        existing.CategoryName = category.CategoryName.Trim();
        await _categoryRepository.UpdateAsync(existing);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);

        if (category is null || await _categoryRepository.HasProductsAsync(id))
        {
            return false;
        }

        await _categoryRepository.DeleteAsync(category);
        return true;
    }

    public Task<bool> HasProductsAsync(int id)
    {
        return _categoryRepository.HasProductsAsync(id);
    }
}
