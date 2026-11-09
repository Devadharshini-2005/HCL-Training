using ProductManagementMVC.Models;

namespace ProductManagementMVC.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Category category);
    Task<bool> UpdateAsync(int id, Category category);
    Task<bool> DeleteAsync(int id);
    Task<bool> HasProductsAsync(int id);
}
