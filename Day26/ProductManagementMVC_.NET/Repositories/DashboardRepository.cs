using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Data;
using ProductManagementMVC.Repositories.Interfaces;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> GetStatisticsAsync()
    {
        var productStats = await _context.Products
            .GroupBy(_ => 1)
            .Select(group => new
            {
                TotalProducts = group.Count(),
                InventoryValue = group.Sum(p => p.Price),
                AveragePrice = group.Average(p => p.Price),
                MostExpensiveProduct = group
                    .OrderByDescending(p => p.Price)
                    .Select(p => p.ProductName)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        var categoryCounts = await _context.Categories
            .Select(c => new CategoryProductCountViewModel
            {
                CategoryName = c.CategoryName,
                ProductCount = c.Products.Count()
            })
            .OrderByDescending(c => c.ProductCount)
            .ToListAsync();

        return new DashboardViewModel
        {
            TotalProducts = productStats?.TotalProducts ?? 0,
            TotalCategories = await _context.Categories.CountAsync(),
            InventoryValue = productStats?.InventoryValue ?? 0,
            AveragePrice = productStats?.AveragePrice ?? 0,
            MostExpensiveProduct = productStats?.MostExpensiveProduct,
            CategoryCounts = categoryCounts
        };
    }
}
