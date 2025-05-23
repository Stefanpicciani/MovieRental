using Microsoft.AspNetCore.Mvc;
using MovieRental.Interfaces.Rentals;
using MovieRental.Models.Rentals;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalController : ControllerBase
    {

        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }

        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Rental rental)
        {
            try
            {
                var result = await _features.SaveWithPaymentAsync(rental);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar rental: {ex.Message}");
            }
        }



        [HttpGet]
        [ActionName("RentalsByCustomer")]
        public async Task<IActionResult> GetByCustomerName(string customerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    return BadRequest("Nome do cliente é obrigatório");
                }

                var rentals = await _features.GetRentalsByCustomerNameAsync(customerName);

                if (!rentals.Any())
                {
                    return NotFound($"Nenhum aluguel encontrado para o cliente: {customerName}");
                }

                return Ok(rentals);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar rentals: {ex.Message}");
            }
        }
    }
}
