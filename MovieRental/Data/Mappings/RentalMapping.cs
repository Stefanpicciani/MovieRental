using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieRental.Models.Rentals;

namespace MovieRental.Data.Mappings
{
    public class RentalMapping : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("Rentals");

            builder.HasOne(r => r.Movie)
                   .WithMany()
                   .HasForeignKey(r => r.MovieId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.TotalPrice)
                .HasColumnType("decimal(10,2)");
        }
    }
}
