using CinemaTicketBookingSystem.Data;
using CinemaTicketBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nowShowing = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Where(m => m.Status == MovieStatus.NowShowing)
                .OrderByDescending(m => m.DateTime)
                .Take(8)
                .ToListAsync();

            var comingSoon = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Where(m => m.Status == MovieStatus.ComingSoon)
                .OrderBy(m => m.DateTime)
                .Take(8)
                .ToListAsync();

            ViewBag.ComingSoon = comingSoon;
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(nowShowing);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
