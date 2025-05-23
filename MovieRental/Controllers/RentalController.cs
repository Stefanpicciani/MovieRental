using Microsoft.AspNetCore.Mvc;
using MovieRental.Interfaces.Rentals;

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


        //[HttpPost]
        //public IActionResult Post([FromBody] Rental rental)
        //{
	       // return Ok(_features.Save(rental));
        //}

	}
}
