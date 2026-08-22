using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingSystem.Helpers
{
    public static class EnumExtensions
    {
        // يرجع النص العربي الموضوع في [Display(Name = "...")] فوق قيمة الـ enum
        public static string GetDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var attr = member?.GetCustomAttributes(typeof(DisplayAttribute), false)
                              .Cast<DisplayAttribute>()
                              .FirstOrDefault();
            return attr?.Name ?? value.ToString();
        }
    }
}
