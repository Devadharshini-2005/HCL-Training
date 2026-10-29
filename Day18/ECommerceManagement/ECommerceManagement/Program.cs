using ECommerceManagement.Models;
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
            Console.WriteLine("3.Customer Management");
            Console.WriteLine("4.Shopping Cart");
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
                    await ProductManagement();
                    break;
                case 3:
                    await CustomerManagement();
                    break;
                case 4:
                    // Shopping Cart logic
                    break;
                case 5:
                    await OrderManagement();
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

    static async Task ProductManagement()
    {
        ProductService productService = new ProductService();

        while (true)
        {
            Console.WriteLine("-------------Product Management--------------");
            Console.WriteLine("1.Add Product");
            Console.WriteLine("2.Update Product");
            Console.WriteLine("3.Delete Product");
            Console.WriteLine("4.View All Products");
            Console.WriteLine("5.Search Product by ID\n" +
                "6.View Products by Category\n" +
                "7.View Low-Stock Products");
            Console.WriteLine("8.Back to Main Menu");
            Console.WriteLine("Enter your choice");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    await productService.AddProduct();
                    break;
                case 2:
                    await productService.UpdateProduct();
                    break;
                case 3:
                    await productService.DeleteProduct();
                    break;
                case 4:
                    await productService.ViewAllProducts();
                    break;
                case 5:
                    await productService.SearchProductById();
                    break;
                case 6:
                    await productService.ViewProductsByCategory();
                    break;
                case 7:
                    await productService.ViewLowStockProducts();
                    break;
                case 8:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task OrderManagement()
    {
        OrderService orderService = new OrderService();
        while (true)
        {
            Console.WriteLine("-------------Order Management--------------");
            Console.WriteLine("1.Place Order");
            Console.WriteLine("2.Update Order Status");
            Console.WriteLine("3.View All Orders");
            Console.WriteLine("4.View Orders by Customer ID");
            Console.WriteLine("5.Delete Order");
            Console.WriteLine("6.Back to Main Menu");
            Console.WriteLine("Enter your choice");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter Customer ID:");
                    int customerId = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter Total Amount:");
                    decimal totalAmount = Convert.ToDecimal(Console.ReadLine());
                    await orderService.PlaceOrder(customerId, totalAmount);
                    break;
                case 2:
                    await orderService.UpdateOrderStatus();
                    break;
                case 3:
                    await orderService.ViewAllOrders();
                    break;
                case 4:
                    await orderService.ViewOrdersByCustomerId();
                    break;
                case 5:
                    await orderService.DeleteOrder();
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task CustomerManagement()
    {
        CustomerService customerService = new CustomerService();

        while (true)
        {
            Console.WriteLine("---------Customer Management-----------");
            Console.WriteLine("1.Add Customer");
            Console.WriteLine("2.Update Customer by Id");
            Console.WriteLine("3.Delete Customer");
            Console.WriteLine("4.View Customer");
            Console.WriteLine("5.Back to Main menu");

            Console.WriteLine("Enter the Choice : ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
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