using ECommerceManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceManagement.Models;

namespace ECommerceManagement.Services
{
    internal class CategoryService
    {

        Categoryrepository categoryrepository = new Categoryrepository();
        Category category1 = new Category();
        public async Task AddCategory()
        {
            Console.WriteLine("Add Category");
            Console.WriteLine("Enter Category Name");
            string? categoryName = Console.ReadLine();
            Console.WriteLine("Enter Category Description");
            string? categoryDescription = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoryName) || string.IsNullOrWhiteSpace(categoryDescription))
            {
                Console.WriteLine("Invalid category information. Please try again.");
                return;
            }
            Category category = new Category(category1.CategoryId++,categoryName,categoryDescription,category1.IsActive);

            await categoryrepository.AddCategoryAsync(category);

            Console.WriteLine($"Category '{categoryName}' added successfully.");
        }

        public async Task UpdateCategory()
        {
            Console.WriteLine("Update Category");
            Console.WriteLine("Enter Category Id");
            int categoryId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter New Category Name");
            string? newCategoryName = Console.ReadLine();
            Console.WriteLine("Enter New Category Description");
            string? newCategoryDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newCategoryName) || string.IsNullOrWhiteSpace(newCategoryDescription))
            {
                Console.WriteLine("Invalid category information. Please try again.");
                return;
            }

            await categoryrepository.UpdateCategoryAsync(categoryId, newCategoryName, newCategoryDescription);
            Console.WriteLine($"Category with ID '{categoryId}' updated successfully.");
        }

        public async Task DeleteCategory()
        {
            Console.WriteLine("Delete Category");
            Console.WriteLine("Enter Category Id");
            int categoryId = Convert.ToInt32(Console.ReadLine());
            await categoryrepository.DeleteCategoryAsync(categoryId);
            Console.WriteLine($"Category with ID '{categoryId}' deleted successfully.");
        }

        public async Task ViewAllCategories()
        {
            var categories = await categoryrepository.GetAllCategoriesAsync();
            Console.WriteLine("All Categories:");
            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.CategoryId}, Name: {category.CategoryName}, Description: {category.Description}");
            }
        }

        public async Task<bool> GetCategoryById(int categoryId)
        {
            
  
            var category = await categoryrepository.GetCategoryByIdAsync(categoryId);

            if (category != null)
            { 
                return true;    
            }
            else
            {
                return false;
            }
        }
    }
}
