namespace MovieRental.API.Models.Payment
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

        public static PaymentResult SuccessResult(string transactionId)
        {
            return new PaymentResult
            {
                Success = true,
                TransactionId = transactionId
            };
        }

        public static PaymentResult FailureResult(string errorMessage)
        {
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
