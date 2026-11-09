using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Data;
using ProductManagementMVC.Models;
using ProductManagementMVC.Repositories.Interfaces;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductListViewModel> GetPagedAsync(
        string? searchTerm,
        int? categoryId,
        string sortOrder,
        int pageNumber,
        int pageSize)
    {
        IQueryable<Product> query = _context.Products
            .AsNoTracking()
            .Include(p => p.Category);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string term = searchTerm.Trim();
            query = query.Where(p => p.ProductName.Contains(term));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        query = sortOrder switch
        {
            "name_desc" => query.OrderByDescending(p => p.ProductName),
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "category_asc" => query.OrderBy(p => p.Category!.CategoryName),
            "category_desc" => query.OrderByDescending(p => p.Category!.CategoryName),
            _ => query.OrderBy(p => p.ProductId)
        };

        int totalRecords = await query.CountAsync();

        List<Product> products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ProductListViewModel
        {
            Products = products,
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            SortOrder = sortOrder
        };
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public Task<bool> CategoryExistsAsync(int categoryId)
    {
        return _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
    }
}
