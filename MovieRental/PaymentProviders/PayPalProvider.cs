using MovieRental.API.Models.Payment;
using MovieRental.Interfaces.Payment;

namespace MovieRental.PaymentProviders
{
    public class PayPalProvider : IPaymentProvider
    {
        public string PaymentMethod => "PayPal";
        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount)
        {
            try
            {
                // Simulação de processamento assíncrono
                await Task.Delay(1000);

                // Lógica específica do PayPal aqui
                bool paymentSuccess = await Pay((double)amount);

                if (paymentSuccess)
                {
                    return PaymentResult.SuccessResult($"PP_{Guid.NewGuid():N}");
                }

                return PaymentResult.FailureResult("Falha no processamento PayPal");
            }
            catch (Exception ex)
            {
                return PaymentResult.FailureResult($"Erro PayPal: {ex.Message}");
            }
        }

        public Task<bool> Pay(double price)
        {
            //ignore this implementation
            return Task.FromResult<bool>(true);
        }
    }
}
