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

            Product product = new Product(productName, description, price, stockQuantity, categoryId);

            await productRepository.AddProduct(product);

            Console.WriteLine($"Product '{product.ProductName}' added successfully.");
        }
    }
}
