using MovieRental.Models.Movies;
using MovieRental.Interfaces.Movies;

namespace MovieRental.Features.Movies
{
    public class MovieFeatures : IMovieFeatures
    {
        public Task<IEnumerable<Movie>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            throw new NotImplementedException();
        }

        public Task<Movie?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Movie> SaveAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> SearchByTitleAsync(string title, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }
    }
}
