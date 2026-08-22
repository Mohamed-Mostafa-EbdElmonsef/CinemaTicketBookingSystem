using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingSystem.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(100)]
        [Display(Name = "اسم التصنيف")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
