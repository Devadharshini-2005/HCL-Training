
using ECommerceManagement.Repositories;

namespace ECommerceManagement.Services
{
    internal class CustomerService
    {
        private readonly CustomerRepository customerRepository =
            new CustomerRepository();

        public async Task<bool> GetCustomerById(int customerId)
        {
            var customer =
                await customerRepository.GetCustomerById(customerId);

            return customer != null;
        }
    }
}
