using ProductManagementMVC.Models;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Services.Interfaces;

public interface IProductService
{
    Task<ProductListViewModel> GetProductsAsync(
        string? searchTerm,
        int? categoryId,
        string sortOrder,
        int pageNumber,
        int pageSize);

    Task<Product?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Product product);
    Task<bool> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
}
