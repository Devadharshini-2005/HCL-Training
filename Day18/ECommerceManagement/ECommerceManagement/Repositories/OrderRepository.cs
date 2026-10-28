using ECommerceManagement.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceManagement.Models;

namespace ECommerceManagement.Repositories
{
    internal class OrderRepository
    {
        public async Task AddOrderAsync(Order order)
        {
            var connection = DatabaseHelper.CreateConnection();

            await connection.OpenAsync();

            using SqlCommand command = new SqlCommand("INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) VALUES (@CustomerId, @OrderDate, @TotalAmount)", connection);

            command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

            await command.ExecuteNonQueryAsync();

            Console.WriteLine($"Order with ID {order.OrderId} added to the repository.");
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("UPDATE Orders SET Status = @NewStatus WHERE OrderId = @OrderId", connection);
            command.Parameters.AddWithValue("@NewStatus", newStatus);
            command.Parameters.AddWithValue("@OrderId", orderId);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"Order with ID '{orderId}' updated to status '{newStatus}' in the repository.");
            }
            else
            {
                Console.WriteLine($"No order found with ID '{orderId}'.");
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Orders", connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Order> orders = new List<Order>();
            while (await reader.ReadAsync())
            {
                Order order = new Order(
                    Convert.ToInt32(reader["CustomerId"]),
                    Convert.ToDateTime(reader["OrderDate"]),
                    Convert.ToDecimal(reader["TotalAmount"])
                )
                {
                    OrderId = Convert.ToInt32(reader["OrderId"]),
                    Status = reader["Status"].ToString() ?? string.Empty
                };
                orders.Add(order);
            }
            return orders;
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE CustomerId = @CustomerId", connection);
            command.Parameters.AddWithValue("@CustomerId", customerId);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Order> orders = new List<Order>();
            while (await reader.ReadAsync())
            {
                Order order = new Order(
                    Convert.ToInt32(reader["CustomerId"]),
                    Convert.ToDateTime(reader["OrderDate"]),
                    Convert.ToDecimal(reader["TotalAmount"])
                )
                {
                    OrderId = Convert.ToInt32(reader["OrderId"]),
                    Status = reader["Status"].ToString() ?? string.Empty
                };
                orders.Add(order);
            }
            return orders;
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var connection = DatabaseHelper.CreateConnection();
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderId = @OrderId", connection);
            command.Parameters.AddWithValue("@OrderId", orderId);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"Order with ID '{orderId}' deleted from the repository.");
            }
            else
            {
                Console.WriteLine($"No order found with ID '{orderId}'.");
            }
        }
    }
}
