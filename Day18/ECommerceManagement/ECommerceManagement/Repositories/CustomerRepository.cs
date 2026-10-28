
using ECommerceManagement.Helpers;
using ECommerceManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceManagement.Repositories
{
    internal class CustomerRepository
    {
        public async Task<Customer?> GetCustomerById(int customerId)
        {
            using SqlConnection connection =
                DatabaseHelper.CreateConnection();

            await connection.OpenAsync();

            string query = @"
                SELECT CustomerId, FullName, Email, Phone,
                       Address, CreatedAt, IsActive
                FROM Customers
                WHERE CustomerId = @CustomerId";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@CustomerId", SqlDbType.Int)
                              .Value = customerId;

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Customer customer = new Customer
                {
                    CustomerId = reader.GetInt32(
                        reader.GetOrdinal("CustomerId")),

                    FullName = reader.GetString(
                        reader.GetOrdinal("FullName")),

                    Email = reader.GetString(
                        reader.GetOrdinal("Email")),

                    Phone = reader.GetString(
                        reader.GetOrdinal("Phone")),

                    Address = reader.GetString(
                        reader.GetOrdinal("Address")),

                    CreatedAt = reader.GetDateTime(
                        reader.GetOrdinal("CreatedAt")),

                    IsActive = reader.GetBoolean(
                        reader.GetOrdinal("IsActive"))
                };

                return customer;
            }

            return null;
        }
    }
}
