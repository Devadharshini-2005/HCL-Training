using ECommerceManagement.Models;
using ECommerceManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Services
{
    internal class ProductService
    {
        ProductRepository productRepository = new ProductRepository();

        CategoryService categoryService = new CategoryService();
        public async Task AddProduct()
        {
            Console.WriteLine("Enter Product Name:");
            string? productName = Console.ReadLine();
            Console.WriteLine("Enter Product Description:");
            string? description = Console.ReadLine();
            Console.WriteLine("Enter Product Price:");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter Product Stock Quantity:");
            int stockQuantity = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Product Category Id:");
            int categoryId = Convert.ToInt32(Console.ReadLine());

            if (!(await categoryService.GetCategoryById(categoryId)))
            {
                Console.WriteLine("Invalid category ID. Please try again.");
                return;
            }

            Product product = new Product(productName, description, price, stockQuantity, categoryId);

            await productRepository.AddProductAsync(product);

            Console.WriteLine($"Product '{product.ProductName}' added successfully.");
        }

        public async Task UpdateProduct()
        {
            Console.WriteLine("Enter Product Id to update:");
            int productId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter new Product Name:");
            string? newProductName = Console.ReadLine();
            Console.WriteLine("Enter new Product Description:");
            string? newDescription = Console.ReadLine();
            Console.WriteLine("Enter new Product Price:");
            decimal newPrice = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter new Product Stock Quantity:");
            int newStockQuantity = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter new Product Category Id:");
            int newCategoryId = Convert.ToInt32(Console.ReadLine());

            await productRepository.UpdateProductAsync(productId, newProductName, newDescription, newPrice, newStockQuantity, newCategoryId);
            Console.WriteLine($"Product with ID '{productId}' updated successfully.");
        }

        public async Task DeleteProduct()
        {
            Console.WriteLine("Enter Product Id to delete:");
            int productId = Convert.ToInt32(Console.ReadLine());
            await productRepository.DeleteProductAsync(productId);
        }

        public async Task ViewAllProducts()
        {
            var products = await productRepository.GetAllProductsAsync();
            Console.WriteLine("Product List:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Description: {product.Description}, Price: {product.Price}, Stock: {product.StockQuantity}, Category ID: {product.CategoryId}, Active: {product.IsActive}, Created At: {product.CreatedAt}");
            }
        }

        public async Task SearchProductById()
        {
            Console.WriteLine("Enter Product Id to search:");
            int productId = Convert.ToInt32(Console.ReadLine());
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product != null)
            {
                Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Description: {product.Description}, Price: {product.Price}, Stock: {product.StockQuantity}, Category ID: {product.CategoryId}, Active: {product.IsActive}, Created At: {product.CreatedAt}");
            }
            else
            {
                Console.WriteLine($"No product found with ID '{productId}'.");
            }
        }

        public async Task ViewProductsByCategory()
        {
            Console.WriteLine("Enter Category Id to view products:");
            int categoryId = Convert.ToInt32(Console.ReadLine());
            var products = await productRepository.GetProductsByCategoryAsync(categoryId);
            if (products.Count > 0)
            {
                Console.WriteLine($"Products in Category ID '{categoryId}':");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Description: {product.Description}, Price: {product.Price}, Stock: {product.StockQuantity}, Active: {product.IsActive}, Created At: {product.CreatedAt}");
                }
            }
            else
            {
                Console.WriteLine($"No products found in Category ID '{categoryId}'.");
            }
        }

        public async Task ViewLowStockProducts()
        {
            Console.WriteLine("Enter stock threshold to view low-stock products:");
            int stockThreshold = Convert.ToInt32(Console.ReadLine());
            var products = await productRepository.GetLowStockProductsAsync(stockThreshold);

            if (products.Count > 0)
            {
                Console.WriteLine($"Products with stock below '{stockThreshold}':");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Description: {product.Description}, Price: {product.Price}, Stock: {product.StockQuantity}, Category ID: {product.CategoryId}, Active: {product.IsActive}, Created At: {product.CreatedAt}");
                }
            }
            else
            {
                Console.WriteLine($"No products found with stock below '{stockThreshold}'.");
            }
        }
    }
}
