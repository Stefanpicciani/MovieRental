using MovieRental.API.Models.Payment;

namespace MovieRental.Interfaces.Payment
{
    public interface IPaymentProvider
    {
        string PaymentMethod { get; }
        Task<PaymentResult> ProcessPaymentAsync(decimal amount);
    }
}
