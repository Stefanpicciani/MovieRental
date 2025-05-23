using Microsoft.AspNetCore.Mvc;
using MovieRental.Interfaces.Payment;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetAvailablePaymentMethods()
        {
            var methods = _paymentService.GetAvailablePaymentMethods();
            return Ok(new { paymentMethods = methods });
        }
    }
}
