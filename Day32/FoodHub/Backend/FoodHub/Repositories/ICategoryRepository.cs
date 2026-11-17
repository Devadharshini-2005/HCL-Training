
using FoodHub.Models;

namespace FoodHub.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category> CreateAsync(Category category);

    Task<Category?> GetByIdAsync(int id);

    Task<Category?> UpdateAsync(int id, Category category);
}
