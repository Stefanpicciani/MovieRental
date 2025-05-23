using Microsoft.AspNetCore.Mvc;
using MovieRental.API.Models.Customers;
using MovieRental.Interfaces.Customers;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerFeatures _customerFeatures;

        public CustomerController(ICustomerFeatures customerFeatures)
        {
            _customerFeatures = customerFeatures;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var customers = await _customerFeatures.GetAllAsync(page, pageSize);
            return Ok(new { data = customers, page, pageSize });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var customer = await _customerFeatures.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound($"Cliente com ID {id} não encontrado");
            }

            return Ok(customer);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var customer = await _customerFeatures.GetByEmailAsync(email);

            if (customer == null)
            {
                return NotFound($"Cliente com email {email} não encontrado");
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var existingCustomer = await _customerFeatures.GetByEmailAsync(customer.Email);
            if (existingCustomer != null)
            {
                return Conflict("Email já está em uso");
            }

            var result = await _customerFeatures.SaveAsync(customer);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest("ID do parâmetro não confere com o ID do objeto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _customerFeatures.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound($"Cliente com ID {id} não encontrado");
            }

            var result = await _customerFeatures.SaveAsync(customer);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                var deleted = await _customerFeatures.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound($"Cliente com ID {id} não encontrado");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
