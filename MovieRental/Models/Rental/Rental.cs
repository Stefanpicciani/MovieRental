using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieRental.API.Models.Customers;
using MovieRental.Models.Movies;

namespace MovieRental.Models.Rentals
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 365)]
        public int DaysRented { get; set; }

        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }
        public virtual Movie? Movie { get; set; }
        
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;
        
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        public bool IsReturned => ReturnDate.HasValue;

        
        [NotMapped]
        public DateTime DueDate => RentalDate.AddDays(DaysRented);

        [NotMapped]
        public bool IsOverdue => !IsReturned && DateTime.UtcNow > DueDate;
    }
}
