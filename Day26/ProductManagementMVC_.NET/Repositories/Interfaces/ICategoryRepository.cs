using ProductManagementMVC.Models;

namespace ProductManagementMVC.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> ExistsByNameAsync(string categoryName, int? excludedId = null);
    Task<bool> HasProductsAsync(int categoryId);
    Task<int> CountAsync();
}
