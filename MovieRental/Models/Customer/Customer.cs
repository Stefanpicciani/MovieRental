using System.ComponentModel.DataAnnotations;
using MovieRental.API.Models.Rentals;
using MovieRental.Models.Rentals;

namespace MovieRental.API.Models.Customers
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
                

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;                
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
