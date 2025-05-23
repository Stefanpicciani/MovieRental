using MovieRental.API.Models.Customers;
using MovieRental.Interfaces.Customers;

namespace MovieRental.Features.Customers
{
    public class CustomerFeatures : ICustomerFeatures
    {
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> SaveAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
