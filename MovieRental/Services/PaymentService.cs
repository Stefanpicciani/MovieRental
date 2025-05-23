using MovieRental.API.Models.Payment;
using MovieRental.Interfaces.Payment;

namespace MovieRental.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly Dictionary<string, IPaymentProvider> _paymentProviders;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IEnumerable<IPaymentProvider> paymentProviders,
                            ILogger<PaymentService> logger)
        {
            _logger = logger;
            _paymentProviders = paymentProviders.ToDictionary(p => p.PaymentMethod.ToUpper(), p => p, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                return PaymentResult.FailureResult("Método de pagamento não especificado");
            }

            if (amount <= 0)
            {
                return PaymentResult.FailureResult("Valor deve ser maior que zero");
            }

            if (!_paymentProviders.TryGetValue(paymentMethod.ToUpper(), out var provider))
            {
                _logger.LogWarning("Método de pagamento não suportado: {PaymentMethod}", paymentMethod);
                return PaymentResult.FailureResult($"Método de pagamento '{paymentMethod}' não suportado");
            }

            try
            {
                _logger.LogInformation("Processando pagamento de {Amount:C} via {PaymentMethod}", amount, paymentMethod);

                var result = await provider.ProcessPaymentAsync(amount);

                if (result.Success)
                {
                    _logger.LogInformation("Pagamento processado com sucesso. TransactionId: {TransactionId}", result.TransactionId);
                }
                else
                {
                    _logger.LogWarning("Falha no pagamento: {ErrorMessage}", result.ErrorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado no processamento do pagamento");
                return PaymentResult.FailureResult("Erro interno no processamento do pagamento");
            }
        }

        public IEnumerable<string> GetAvailablePaymentMethods()
        {
            return _paymentProviders.Keys;
        }
    }
}
