using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ECommerceManagement.Helpers;
using ECommerceManagement.Models;
using Microsoft.Data.SqlClient;

namespace ECommerceManagement.Repositories
{
    internal class Categoryrepository
    {
        public async Task AddCategoryAsync(Category category)
        {
            var connection = DatabaseHelper.CreateConnection();

            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description)", connection);

            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            command.Parameters.AddWithValue("@Description", category.Description);

            await command.ExecuteNonQueryAsync();

            Console.WriteLine($"Category '{category.CategoryName}' with description '{category.Description}' added to the repository.");
        }

        public async Task UpdateCategoryAsync(int categoryId, string newCategoryName, string newCategoryDescription)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UPDATE Categories SET CategoryName = @NewCategoryName, Description = @NewCategoryDescription WHERE CategoryId = @CategoryId", connection);
           
            command.Parameters.AddWithValue("@NewCategoryName", newCategoryName);
            command.Parameters.AddWithValue("@NewCategoryDescription", newCategoryDescription);
            command.Parameters.AddWithValue("@CategoryId", categoryId);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            
            if (rowsAffected > 0)
            {
                Console.WriteLine($"Category with ID '{categoryId}' updated in the repository.");
            }
            else
            {
                Console.WriteLine($"No category found with ID '{categoryId}'.");
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DELETE FROM Categories WHERE CategoryId = @CategoryId", connection);

            command.Parameters.AddWithValue("@CategoryId", categoryId);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"Category with ID '{categoryId}' deleted from the repository.");
            }
            else
            {
                Console.WriteLine($"No category found with ID '{categoryId}'.");
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Categories", connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            List<Category> categories = new List<Category>();

            while (await reader.ReadAsync())
            {
                Category category = new Category(
                    reader["CategoryName"].ToString() ?? string.Empty,
                    reader["Description"].ToString() ?? string.Empty
                )
                {
                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                };
                categories.Add(category);
            }
            return categories;
        }
    }
}
