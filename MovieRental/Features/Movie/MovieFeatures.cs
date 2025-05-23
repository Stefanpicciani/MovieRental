using MovieRental.Models.Movies;
using MovieRental.Interfaces.Movies;
using MovieRental.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MovieRental.Features.Movies
{
    public class MovieFeatures : IMovieFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;

        public MovieFeatures(MovieRentalDbContext movieRentalDb)
        {
            _movieRentalDb = movieRentalDb;
        }

        public async Task<Movie> SaveAsync(Movie movie)
        {
            await _movieRentalDb.Movies.AddAsync(movie);
            await _movieRentalDb.SaveChangesAsync();
            return movie;
        }
        
        
        public async Task<IEnumerable<Movie>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            return await _movieRentalDb.Movies
                .OrderBy(m => m.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking() 
                .ToListAsync();
        }

        
        public async Task<int> GetTotalCountAsync()
        {
            return await _movieRentalDb.Movies.CountAsync();
        }

                
        public async Task<IEnumerable<Movie>> SearchByTitleAsync(string title, int page = 1, int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Enumerable.Empty<Movie>();

            return await _movieRentalDb.Movies
                .Where(m => m.Title.ToLower().Contains(title.ToLower()))
                .OrderBy(m => m.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        
        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _movieRentalDb.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
