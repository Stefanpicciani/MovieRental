using MovieRental.API.Models.Payment;

namespace MovieRental.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, decimal amount);
        IEnumerable<string> GetAvailablePaymentMethods();
    }
}
