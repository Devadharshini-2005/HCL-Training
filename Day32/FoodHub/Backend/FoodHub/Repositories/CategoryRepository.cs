
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

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.Categories.AddAsync(category);

        await _context.SaveChangesAsync();

        return category;
    }


    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.CategoryId == id);
    }

    public async Task<Category?> UpdateAsync(int id, Category category)
    {
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (existingCategory == null)
        {
            return null;
        }

        existingCategory.CategoryName = category.CategoryName;
        existingCategory.Description = category.Description;

        await _context.SaveChangesAsync();

        return existingCategory;
    }


}
