

using MovieRental.Models.Movies;

namespace MovieRental.Interfaces.Movies
{
    public interface IMovieFeatures
    {
        // Métodos assíncronos (recomendados)
        Task<Movie> SaveAsync(Movie movie);
        Task<IEnumerable<Movie>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<Movie>> SearchByTitleAsync(string title, int page = 1, int pageSize = 20);
        Task<Movie?> GetByIdAsync(int id);
    }
}
