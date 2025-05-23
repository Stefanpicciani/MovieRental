using MovieRental.API.Models.Payment;
using MovieRental.Interfaces.Payment;

namespace MovieRental.Services
{
    public class PaymentService : IPaymentService
    {
        public IEnumerable<string> GetAvailablePaymentMethods()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
