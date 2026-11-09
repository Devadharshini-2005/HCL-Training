using ProductManagementMVC.Models;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Repositories.Interfaces;

public interface IProductRepository
{
    Task<ProductListViewModel> GetPagedAsync(
        string? searchTerm,
        int? categoryId,
        string sortOrder,
        int pageNumber,
        int pageSize);

    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> CategoryExistsAsync(int categoryId);
}
