using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MovieRental.API.Models.Customers;
using MovieRental.API.Models.Rentals;
using MovieRental.Data.Mappings;
using MovieRental.Models.Movies;
using MovieRental.Models.Rentals;
using System.Reflection.Metadata;

namespace MovieRental.Data.Context
{
	public class MovieRentalDbContext : DbContext
	{
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Customer> Customers { get; set; }

        private string DbPath { get; }

		public MovieRentalDbContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = Path.Join(path, "movierental.db");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MovieMapping());
            modelBuilder.ApplyConfiguration(new RentalMapping());
            modelBuilder.ApplyConfiguration(new CustomerMapping());
        }
    }
}
