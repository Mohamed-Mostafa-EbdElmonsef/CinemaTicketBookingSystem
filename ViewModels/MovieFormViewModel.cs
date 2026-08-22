using System.ComponentModel.DataAnnotations;
using CinemaTicketBookingSystem.Models;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketBookingSystem.ViewModels
{
    // يُستخدم في شاشات إضافة/تعديل الفيلم في لوحة التحكم
    public class MovieFormViewModel
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
        [Range(0, 100000)]
        [Display(Name = "سعر التذكرة")]
        public decimal Price { get; set; }

        [Display(Name = "الحالة")]
        public MovieStatus Status { get; set; }

        [Required]
        [Display(Name = "تاريخ ووقت العرض")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; } = System.DateTime.Now;

        [Display(Name = "الصورة الرئيسية")]
        public IFormFile? MainImgFile { get; set; }
        public string? ExistingMainImg { get; set; }

        [Display(Name = "صور فرعية إضافية")]
        public List<IFormFile>? SubImageFiles { get; set; }
        public List<MovieImage>? ExistingSubImages { get; set; }

        [Required]
        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "السينما")]
        public int CinemaId { get; set; }

        [Display(Name = "الممثلون")]
        public List<int> SelectedActorIds { get; set; } = new();

        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Cinema>? Cinemas { get; set; }
        public IEnumerable<Actor>? Actors { get; set; }
    }
}
