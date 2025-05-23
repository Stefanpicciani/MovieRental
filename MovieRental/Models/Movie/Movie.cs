using System.ComponentModel.DataAnnotations;

namespace MovieRental.Models.Movies
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
