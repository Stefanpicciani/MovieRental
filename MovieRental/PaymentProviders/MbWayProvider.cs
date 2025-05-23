using MovieRental.API.Models.Payment;
using MovieRental.Interfaces.Payment;

namespace MovieRental.PaymentProviders
{
    public class MbWayProvider : IPaymentProvider
    {
        public string PaymentMethod => "MbWay";

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount)
        {
            try
            {
                // Simulação de processamento assíncrono
                await Task.Delay(800);

                // Lógica específica do MB Way aqui
                bool paymentSuccess = await Pay((double)amount);

                if (paymentSuccess)
                {
                    return PaymentResult.SuccessResult($"MB_{Guid.NewGuid():N}");
                }

                return PaymentResult.FailureResult("Falha no processamento MB Way");
            }
            catch (Exception ex)
            {
                return PaymentResult.FailureResult($"Erro MB Way: {ex.Message}");
            }
        }

        public Task<bool> Pay(double price)
        {
            //ignore this implementation
            return Task.FromResult<bool>(true);
        }
    }
}
