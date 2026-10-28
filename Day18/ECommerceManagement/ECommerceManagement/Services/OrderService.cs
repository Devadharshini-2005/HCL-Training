using System;
using System.Collections.Generic;
using System.Text;
using ECommerceManagement.Models;
using ECommerceManagement.Repositories;

namespace ECommerceManagement.Services
{
    internal class OrderService
    {
        OrderRepository orderRepository = new OrderRepository();
        CustomerService customerService = new CustomerService();
        public async Task PlaceOrder(int customerId, decimal totalAmount)
        {

            Order newOrder = new Order(customerId, DateTime.Now, totalAmount);
            
            if(newOrder.TotalAmount <= 0)
            {
                Console.WriteLine("Total amount must be greater than zero.");
                return;
            }

            if(!await customerService.GetCustomerById(customerId))
            {
                Console.WriteLine($"Customer with ID '{customerId}' does not exist. Cannot place order.");
                return;
            }

            Console.WriteLine($"Order placed successfully! Order ID: {newOrder.OrderId}," +
                $" Total Amount: {newOrder.TotalAmount}");

            await orderRepository.AddOrderAsync(newOrder);
        }

        public async Task UpdateOrderStatus()
        {
            Console.WriteLine("Enter Order ID to update status:");
            int orderId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter new status (Placed, Shipped, Delivered, Cancelled):");
            string? newStatus = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newStatus))
            {
                Console.WriteLine("Status cannot be empty.");
                return;
            }
            await orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }

        public async Task ViewAllOrders()
        {
            var orders = await orderRepository.GetAllOrdersAsync();
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer ID: {order.CustomerId}, " +
                    $"Order Date: {order.OrderDate}, Status: {order.Status}, Total Amount: {order.TotalAmount}");
            }
        }

        public async Task ViewOrdersByCustomerId()
        {
            Console.WriteLine("Enter Customer ID to view orders:");
            int customerId = Convert.ToInt32(Console.ReadLine());
            var orders = await orderRepository.GetOrdersByCustomerIdAsync(customerId);
            if (orders.Count == 0)
            {
                Console.WriteLine($"No orders found for Customer ID '{customerId}'.");
                return;
            }
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Order Date: {order.OrderDate}, " +
                    $"Status: {order.Status}, Total Amount: {order.TotalAmount}");
            }
        }

        public async Task DeleteOrder()
        {
            Console.WriteLine("Enter Order ID to delete:");
            int orderId = Convert.ToInt32(Console.ReadLine());
            await orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
