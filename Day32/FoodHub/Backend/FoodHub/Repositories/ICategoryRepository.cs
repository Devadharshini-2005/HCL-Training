using FoodHub.Models;

namespace FoodHub.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
}