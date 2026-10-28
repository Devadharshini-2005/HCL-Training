
using ECommerceManagement.Services;
public class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("-------------E-Commerce Management System--------------");
            Console.WriteLine("1.Category Management");
            Console.WriteLine("2.Product Management");
            Console.WriteLine("3.Order Management");
            Console.WriteLine("4. Shopping Cart");
            Console.WriteLine("5.Order Management");
            Console.WriteLine("6.Reports");
            Console.WriteLine("7.View Audit Logs");
            Console.WriteLine("8.Exit");

            Console.WriteLine("Enter your choice");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    await CategoryManagement();
                    break;
                case 2:
                    // Product Management logic
                    break;
                case 3:
                    // Order Management logic
                    break;
                case 4:
                    // Shopping Cart logic
                    break;
                case 5:
                    // Order Management logic
                    break;
                case 6:
                    // Reports logic
                    break;
                case 7:
                    // View Audit Logs logic
                    break;
                case 8:
                    Console.WriteLine("Exiting the application.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task CategoryManagement()
    {
        CategoryService categoryService = new CategoryService();
        while (true)
        {
            Console.WriteLine("-------------Category Management--------------");
            Console.WriteLine("1.Add Category");
            Console.WriteLine("2.Update Category");
            Console.WriteLine("3.Delete Category");
            Console.WriteLine("4.View All Categories");
            Console.WriteLine("5.Back to Main Menu");

            Console.WriteLine("Enter your choice");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    await categoryService.AddCategory();
                    break;
                case 2:
                    await categoryService.UpdateCategory();
                    break;
                case 3:
                    await categoryService.DeleteCategory();
                    break;
                case 4:
                    await categoryService.ViewAllCategories();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}