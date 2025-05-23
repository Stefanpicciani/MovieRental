using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieRental.Models.Movies;

namespace MovieRental.Data.Mappings
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(200);
        }
    }
}
