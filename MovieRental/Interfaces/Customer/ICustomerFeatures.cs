using MovieRental.API.Models.Customers;

namespace MovieRental.Interfaces.Customers
{
    public interface ICustomerFeatures
    {
        Task<Customer> SaveAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<Customer?> GetByEmailAsync(string email);
        Task<bool> DeleteAsync(int id);
    }
}
