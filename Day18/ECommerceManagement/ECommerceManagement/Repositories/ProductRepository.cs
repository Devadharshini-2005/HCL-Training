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
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("INSERT INTO Products " +
                "(ProductName, Description, Price, StockQuantity, CategoryId, IsActive, CreatedAt) " +
                "VALUES (@ProductName, @Description, @Price, @StockQuantity, @CategoryId, @IsActive, @CreatedAt)", connection);
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
            await connection.OpenAsync();
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

        public async Task DeleteProductAsync(int productId)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UPDATE Products SET IsActive = 0 WHERE ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"Product with ID '{productId}' marked as inactive in the database.");
            }
            else
            {
                Console.WriteLine($"No product found with ID '{productId}'.");
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Products", connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Product> products = new List<Product>();
            while (await reader.ReadAsync())
            {
                Product product = new Product();

                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString() ?? string.Empty;
                product.Description = reader["Description"].ToString();
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                product.IsActive = Convert.ToBoolean(reader["IsActive"]);
                product.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                products.Add(product);
            }
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                Product product = new Product();
                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString() ?? string.Empty;
                product.Description = reader["Description"].ToString();
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                product.IsActive = Convert.ToBoolean(reader["IsActive"]);
                product.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                return product;
            }
            return null;
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE CategoryId = @CategoryId", connection);
            command.Parameters.AddWithValue("@CategoryId", categoryId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Product> products = new List<Product>();
            while (await reader.ReadAsync())
            {
                Product product = new Product();
                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString() ?? string.Empty;
                product.Description = reader["Description"].ToString();
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                product.IsActive = Convert.ToBoolean(reader["IsActive"]);
                product.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                products.Add(product);
            }
            return products;
        }

        public async Task<List<Product>> GetLowStockProductsAsync(int stockThreshold)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE StockQuantity < @StockThreshold", connection);
            command.Parameters.AddWithValue("@StockThreshold", stockThreshold);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Product> products = new List<Product>();
            while (await reader.ReadAsync())
            {
                Product product = new Product();

                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString() ?? string.Empty;
                product.Description = reader["Description"].ToString();
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                product.IsActive = Convert.ToBoolean(reader["IsActive"]);
                product.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                products.Add(product);
            }
            return products;
        }
    }
}
