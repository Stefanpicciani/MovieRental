using Microsoft.EntityFrameworkCore;
using MovieRental.API.Models.Customers;
using MovieRental.Data.Context;
using MovieRental.Interfaces.Customers;

namespace MovieRental.Features.Customers
{
    public class CustomerFeatures : ICustomerFeatures
    {
        private readonly MovieRentalDbContext _context;
        private readonly ILogger<CustomerFeatures> _logger;

        public CustomerFeatures(MovieRentalDbContext context, ILogger<CustomerFeatures> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer> SaveAsync(Customer customer)
        {
            if (customer.Id == 0)
            {
                await _context.Customers.AddAsync(customer);
            }
            else
            {
                _context.Customers.Update(customer);
            }

            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Rentals)
                .ThenInclude(r => r.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            return await _context.Customers
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            // Verificar se tem rentals ativos
            var hasActiveRentals = await _context.Rentals
                .AnyAsync(r => r.CustomerId == id && !r.IsReturned);

            if (hasActiveRentals)
            {
                throw new InvalidOperationException("Não é possível excluir cliente com alugueis ativos");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
