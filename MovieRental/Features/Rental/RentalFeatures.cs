using MovieRental.API.Models.Rentals;
using MovieRental.Interfaces.Rentals;
using MovieRental.Models.Rentals;

namespace MovieRental.Features.Rentals
{
    public class RentalFeatures : IRentalFeatures
    {
        public Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName)
        {
            throw new NotImplementedException();
        }

        public Task<RentalResult> SaveWithPaymentAsync(Rental rental)
        {
            throw new NotImplementedException();
        }
    }
}
