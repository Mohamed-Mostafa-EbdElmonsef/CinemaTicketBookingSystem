using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaTicketBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        [Required(ErrorMessage = "اسم العميل مطلوب")]
        [StringLength(100)]
        [Display(Name = "اسم العميل")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "عدد المقاعد يجب أن يكون بين 1 و 20")]
        [Display(Name = "عدد المقاعد")]
        public int NumberOfSeats { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "السعر الإجمالي")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "تاريخ الحجز")]
        public DateTime BookingDate { get; set; } = System.DateTime.Now;
    }
}
