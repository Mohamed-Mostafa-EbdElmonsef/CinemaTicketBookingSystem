using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingSystem.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم السينما مطلوب")]
        [StringLength(150)]
        [Display(Name = "اسم السينما")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "صورة السينما")]
        public string? Img { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
