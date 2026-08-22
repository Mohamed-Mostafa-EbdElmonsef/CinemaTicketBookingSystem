using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingSystem.Models
{
    // صور فرعية إضافية للفيلم (SubImages)
    public class MovieImage
    {
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; } = string.Empty;

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
