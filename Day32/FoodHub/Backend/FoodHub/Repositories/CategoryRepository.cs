using FoodHub.Data;
using FoodHub.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FoodHubDbContext _context;

    public CategoryRepository(FoodHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }
}