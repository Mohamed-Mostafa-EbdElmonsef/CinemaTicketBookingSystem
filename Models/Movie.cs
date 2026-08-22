using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaTicketBookingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الفيلم مطلوب")]
        [StringLength(150)]
        [Display(Name = "اسم الفيلم")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "الوصف مطلوب")]
        [Display(Name = "الوصف")]
        public string Des { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "سعر التذكرة")]
        [Range(0, 100000, ErrorMessage = "يجب أن يكون السعر رقماً موجباً")]
        public decimal Price { get; set; }

        [Display(Name = "الحالة")]
        public MovieStatus Status { get; set; } = MovieStatus.ComingSoon;

        [Required]
        [Display(Name = "تاريخ ووقت العرض")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; } = System.DateTime.Now;

        [Display(Name = "الصورة الرئيسية")]
        public string? MainImg { get; set; }

        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Display(Name = "السينما")]
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }

        public ICollection<MovieImage> SubImages { get; set; } = new List<MovieImage>();
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
