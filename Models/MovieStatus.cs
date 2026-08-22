using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingSystem.Models
{
    public enum MovieStatus
    {
        [Display(Name = "قريباً")]
        ComingSoon = 0,

        [Display(Name = "يُعرض الآن")]
        NowShowing = 1,

        [Display(Name = "انتهى العرض")]
        Ended = 2
    }
}
