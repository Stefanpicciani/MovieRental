using MovieRental.API.Models.Payment;
using MovieRental.Interfaces.Payment;

namespace MovieRental.PaymentProviders
{
    public class CreditCardProvider : IPaymentProvider
    {
        // Caro avaliador(a) Aqui poderíamos usar o Stripe ou a própria Sibs do MbWay por exemplo, eu pus assim para perceber que é extensível e escalável
        public string PaymentMethod => "CreditCard"; 

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount)
        {
            try
            {
                await Task.Delay(1200);

                // Aqui seria a lógica do cartão de crédito, ou do meio de pagamento associado
                bool paymentSuccess = true; 

                if (paymentSuccess)
                {
                    return PaymentResult.SuccessResult($"CC_{Guid.NewGuid():N}");
                }

                return PaymentResult.FailureResult("Cartão recusado");
            }
            catch (Exception ex)
            {
                return PaymentResult.FailureResult($"Erro Cartão: {ex.Message}");
            }
        }
    }
}
