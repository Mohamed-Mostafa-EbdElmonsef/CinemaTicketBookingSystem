using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingSystem.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الممثل مطلوب")]
        [StringLength(100)]
        [Display(Name = "اسم الممثل")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "صورة الممثل")]
        public string? Img { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
