using ECommerceManagement.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceManagement.Models;

namespace ECommerceManagement.Repositories
{
    internal class ProductRepository
    {
        public async Task AddProductAsync(Product product)
        {
            var connection = DatabaseHelper.CreateConnection();
            using SqlCommand command = new SqlCommand("INSERT INTO Products (ProductName, Description, Price, StockQuantity, CategoryId, IsActive, CreatedAt) VALUES (@ProductName, @Description, @Price, @StockQuantity, @CategoryId, @IsActive, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
            command.Parameters.AddWithValue("@IsActive", product.IsActive);
            command.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);

            await command.ExecuteNonQueryAsync(); 

            Console.WriteLine($"Product '{product.ProductName}' added to the database.");
        }

        public async Task UpdateProductAsync(int productId, string newProductName, string? newDescription, decimal newPrice, int newStockQuantity, int newCategoryId)
        {
            var connection = DatabaseHelper.CreateConnection();

            using SqlCommand command = new SqlCommand("UPDATE Products SET ProductName = @NewProductName, Description = @NewDescription, Price = @NewPrice, StockQuantity = @NewStockQuantity, CategoryId = @NewCategoryId WHERE ProductId = @ProductId", connection);
            
            command.Parameters.AddWithValue("@NewProductName", newProductName);
            command.Parameters.AddWithValue("@NewDescription", (object?)newDescription ?? DBNull.Value);
            command.Parameters.AddWithValue("@NewPrice", newPrice);
            command.Parameters.AddWithValue("@NewStockQuantity", newStockQuantity);
            command.Parameters.AddWithValue("@NewCategoryId", newCategoryId);
            command.Parameters.AddWithValue("@ProductId", productId);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"Product with ID '{productId}' updated in the database.");
            }
            else
            {
                Console.WriteLine($"No product found with ID '{productId}'.");
            }
        }

    }
}
