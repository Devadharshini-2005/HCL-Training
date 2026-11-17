
using FoodHub.Data;
using FoodHub.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Repositories;

public class FoodItemRepository : IFoodItemRepository
{
    private readonly FoodHubDbContext _context;

    public FoodItemRepository(FoodHubDbContext context)
    {
        _context = context;
    }

    // Get all food items
    public async Task<IEnumerable<FoodItem>> GetAllAsync()
    {
        return await _context.FoodItems
            .Include(f => f.Category)
            .ToListAsync();
    }

    // Get food item by ID
    public async Task<FoodItem?> GetByIdAsync(int id)
    {
        return await _context.FoodItems
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.FoodItemId == id);
    }

    // Add food item
    public async Task<FoodItem> AddAsync(FoodItem foodItem)
    {
        await _context.FoodItems.AddAsync(foodItem);
        await _context.SaveChangesAsync();

        return foodItem;
    }

    // Update food item
    public async Task UpdateAsync(FoodItem foodItem)
    {
        _context.FoodItems.Update(foodItem);
        await _context.SaveChangesAsync();
    }

    // Delete food item
    public async Task DeleteAsync(FoodItem foodItem)
    {
        _context.FoodItems.Remove(foodItem);
        await _context.SaveChangesAsync();
    }


    // Search food items by name
    public async Task<IEnumerable<FoodItem>> SearchAsync(string searchTerm)
    {
        return await _context.FoodItems
            .Include(f => f.Category)
            .Where(f => f.FoodName.Contains(searchTerm))
            .ToListAsync();
    }

}
