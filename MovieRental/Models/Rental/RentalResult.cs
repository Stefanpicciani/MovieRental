using MovieRental.Models.Rentals;

namespace MovieRental.API.Models.Rentals
{
    public class RentalResult
    {
        public bool Success { get; set; }
        public Rental? Rental { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }

        public static RentalResult SuccessResult(Rental rental, string transactionId)
        {
            return new RentalResult
            {
                Success = true,
                Rental = rental,
                TransactionId = transactionId
            };
        }

        public static RentalResult FailureResult(string errorMessage)
        {
            return new RentalResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
