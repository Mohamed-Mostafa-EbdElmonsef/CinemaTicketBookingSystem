using CinemaTicketBookingSystem.Data;
using CinemaTicketBookingSystem.Helpers;
using CinemaTicketBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CinemasController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cinemas.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile? imgFile)
        {
            if (!ModelState.IsValid) return View(cinema);

            cinema.Img = await FileHelper.SaveImageAsync(imgFile, _env.WebRootPath, "cinemas");

            _context.Add(cinema);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تمت إضافة السينما بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cinema cinema, IFormFile? imgFile)
        {
            if (id != cinema.Id) return NotFound();
            if (!ModelState.IsValid) return View(cinema);

            var existing = await _context.Cinemas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return NotFound();

            if (imgFile != null)
            {
                FileHelper.DeleteImage(existing.Img, _env.WebRootPath);
                cinema.Img = await FileHelper.SaveImageAsync(imgFile, _env.WebRootPath, "cinemas");
            }
            else
            {
                cinema.Img = existing.Img;
            }

            _context.Update(cinema);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم تعديل السينما بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                var hasMovies = await _context.Movies.AnyAsync(m => m.CinemaId == id);
                if (hasMovies)
                {
                    TempData["Error"] = "لا يمكن حذف هذه السينما لأنها مرتبطة بأفلام موجودة";
                    return RedirectToAction(nameof(Index));
                }

                FileHelper.DeleteImage(cinema.Img, _env.WebRootPath);
                _context.Cinemas.Remove(cinema);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف السينما بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
