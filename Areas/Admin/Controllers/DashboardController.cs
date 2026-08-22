using CinemaTicketBookingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CategoriesCount = await _context.Categories.CountAsync();
            ViewBag.CinemasCount = await _context.Cinemas.CountAsync();
            ViewBag.ActorsCount = await _context.Actors.CountAsync();
            ViewBag.MoviesCount = await _context.Movies.CountAsync();
            ViewBag.BookingsCount = await _context.Bookings.CountAsync();

            ViewBag.RecentBookings = await _context.Bookings
                .Include(b => b.Movie)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .ToListAsync();

            ViewBag.LatestMovies = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .OrderByDescending(m => m.Id)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
