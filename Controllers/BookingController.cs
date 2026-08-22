using CinemaTicketBookingSystem.Data;
using CinemaTicketBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingSystem.Controllers
{
    // حجز التذاكر من الواجهة العامة
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            ViewBag.Movie = movie;
            var booking = new Booking { MovieId = movieId, NumberOfSeats = 1 };
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var movie = await _context.Movies.FindAsync(booking.MovieId);
            if (movie == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Movie = movie;
                return View(booking);
            }

            booking.TotalPrice = movie.Price * booking.NumberOfSeats;
            booking.BookingDate = DateTime.Now;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Confirmation), new { id = booking.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Movie)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }
    }
}
