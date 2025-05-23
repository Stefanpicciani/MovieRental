using Microsoft.EntityFrameworkCore;
using MovieRental.API.Models.Rentals;
using MovieRental.Data.Context;
using MovieRental.Interfaces.Payment;
using MovieRental.Interfaces.Rentals;
using MovieRental.Models.Rentals;
using MovieRental.Utils;

namespace MovieRental.Features.Rentals
{
    public class RentalFeatures : IRentalFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<RentalFeatures> _logger;

        public RentalFeatures(MovieRentalDbContext movieRentalDb,
                            IPaymentService paymentService,
                            ILogger<RentalFeatures> logger)
        {
            _movieRentalDb = movieRentalDb;
            _paymentService = paymentService;
            _logger = logger;
        }

        // Método principal com processamento de pagamento
        public async Task<RentalResult> SaveWithPaymentAsync(Rental rental)
        {
            // IUoW - IUnity of work , caro(a) avaliador(a) pus um IUoW aqui para não termos problemas com consistência de dados
            using var transaction = await _movieRentalDb.Database.BeginTransactionAsync();

            try
            {
                
                var validationResult = await ValidateRentalAsync(rental);
                if (!validationResult.IsValid)
                {
                    return RentalResult.FailureResult(validationResult.ErrorMessage!);
                }

                
                decimal totalPrice = CalculateRentalPrice(rental);
                rental.TotalPrice = totalPrice;

                // Processar pagamento antes de salvar
                _logger.LogInformation("Processando pagamento para rental do cliente {CustomerId}", rental.CustomerId);

                var paymentResult = await _paymentService.ProcessPaymentAsync(rental.PaymentMethod, totalPrice);

                if (!paymentResult.Success)
                {
                    _logger.LogWarning("Falha no pagamento: {ErrorMessage}", paymentResult.ErrorMessage);
                    return RentalResult.FailureResult($"Falha no pagamento: {paymentResult.ErrorMessage}");
                }

                // Se pagamento foi bem-sucedido, salvar o rental
                await _movieRentalDb.Rentals.AddAsync(rental);
                await _movieRentalDb.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Rental criado com sucesso. ID: {RentalId}, TransactionId: {TransactionId}",
                    rental.Id, paymentResult.TransactionId);

                return RentalResult.SuccessResult(rental, paymentResult.TransactionId!);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Erro ao processar rental");
                return RentalResult.FailureResult("Erro interno ao processar rental");
            }
        }

       
        public async Task<IEnumerable<Rental>> GetRentalsByCustomerIdAsync(int customerId)
        {
            return await _movieRentalDb.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.RentalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync(int page = 1, int pageSize = 50)
        {
            return await _movieRentalDb.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .OrderByDescending(r => r.RentalDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        
        public async Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName)
        {
            return await _movieRentalDb.Rentals.Where(x => x.Customer.Name.ToLower().Contains(customerName.ToLower())).ToListAsync();
        }



        
        private async Task<ValidationResult> ValidateRentalAsync(Rental rental)
        {
            if (rental.CustomerId <= 0)
                return ValidationResult.Invalid("ID do cliente é obrigatório");

            if (rental.MovieId <= 0)
                return ValidationResult.Invalid("ID do filme é obrigatório");

            if (rental.DaysRented <= 0)
                return ValidationResult.Invalid("Dias de aluguel deve ser maior que zero");

            if (string.IsNullOrWhiteSpace(rental.PaymentMethod))
                return ValidationResult.Invalid("Método de pagamento é obrigatório");

            
            var customerExists = await _movieRentalDb.Customers
                .AnyAsync(c => c.Id == rental.CustomerId);

            if (!customerExists)
                return ValidationResult.Invalid("Cliente não encontrado");

            
            var movieExists = await _movieRentalDb.Movies
                .AnyAsync(m => m.Id == rental.MovieId);

            if (!movieExists)
                return ValidationResult.Invalid("Filme não encontrado");

            return ValidationResult.Valid();
        }

        private decimal CalculateRentalPrice(Rental rental)
        {
            // Lógica de cálculo de preço (pode ser mais complexa)
            decimal dailyRate = 2.50m; // 2.50 por dia
            decimal totalPrice = rental.DaysRented * dailyRate;


            return Math.Round(totalPrice, 2);
        }

    }
}
