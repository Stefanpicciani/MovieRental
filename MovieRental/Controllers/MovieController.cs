using Microsoft.AspNetCore.Mvc;
using MovieRental.Interfaces.Movies;
using MovieRental.Models.Movies;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MovieController : ControllerBase
    {

        private readonly IMovieFeatures _features;

        public MovieController(IMovieFeatures features)
        {
            _features = features;
        }

        [HttpGet]// Okay
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var movies = await _features.GetAllAsync(page, pageSize);
            var totalCount = await _features.GetTotalCountAsync();

            return Ok(new
            {
                data = movies,
                page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        [HttpGet("{id}")] // Okay
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _features.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound($"Filme com ID {id} não encontrado");
            }

            return Ok(movie);
        }

        [HttpGet("search/{title}")]
        public async Task<IActionResult> SearchByTitleAsync(string title, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var movies = await _features.SearchByTitleAsync(title, page, pageSize);
            return Ok(new { data = movies, searchTerm = title, page, pageSize });
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _features.SaveAsync(movie);
            return await GetByIdAsync(result.Id);
        }
    }
}
