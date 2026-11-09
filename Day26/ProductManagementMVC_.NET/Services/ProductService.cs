using ProductManagementMVC.Models;
using ProductManagementMVC.Repositories.Interfaces;
using ProductManagementMVC.Services.Interfaces;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<ProductListViewModel> GetProductsAsync(
        string? searchTerm,
        int? categoryId,
        string sortOrder,
        int pageNumber,
        int pageSize)
    {
        return _productRepository.GetPagedAsync(
            searchTerm, categoryId, sortOrder, pageNumber, pageSize);
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return _productRepository.GetByIdAsync(id);
    }

    public async Task<bool> CreateAsync(Product product)
    {
        if (!await _productRepository.CategoryExistsAsync(product.CategoryId))
        {
            return false;
        }

        await _productRepository.AddAsync(product);
        return true;
    }

    public async Task<bool> UpdateAsync(int id, Product product)
    {
        Product? existing = await _productRepository.GetByIdAsync(id);

        if (existing is null ||
            !await _productRepository.CategoryExistsAsync(product.CategoryId))
        {
            return false;
        }

        existing.ProductName = product.ProductName;
        existing.Price = product.Price;
        existing.CategoryId = product.CategoryId;

        await _productRepository.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(product);
        return true;
    }
}
