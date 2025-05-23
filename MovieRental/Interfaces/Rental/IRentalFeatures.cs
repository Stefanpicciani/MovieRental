using MovieRental.API.Models.Rentals;
using MovieRental.Models.Rentals;

namespace MovieRental.Interfaces.Rentals
{
    public interface IRentalFeatures
    {
        Task<RentalResult> SaveWithPaymentAsync(Rental rental);
        Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName);
    }
}
