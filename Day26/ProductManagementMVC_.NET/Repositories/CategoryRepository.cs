using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Data;
using ProductManagementMVC.Models;
using ProductManagementMVC.Repositories.Interfaces;

namespace ProductManagementMVC.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.CategoryId)
            .ToListAsync();
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        return _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsByNameAsync(string categoryName, int? excludedId = null)
    {
        IQueryable<Category> query = _context.Categories
            .Where(c => c.CategoryName.ToLower() == categoryName.Trim().ToLower());

        if (excludedId.HasValue)
        {
            query = query.Where(c => c.CategoryId != excludedId.Value);
        }

        return query.AnyAsync();
    }

    public Task<bool> HasProductsAsync(int categoryId)
    {
        return _context.Products.AnyAsync(p => p.CategoryId == categoryId);
    }

    public Task<int> CountAsync()
    {
        return _context.Categories.CountAsync();
    }
}
