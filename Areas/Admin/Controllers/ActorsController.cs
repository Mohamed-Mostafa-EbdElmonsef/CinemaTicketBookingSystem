using CinemaTicketBookingSystem.Data;
using CinemaTicketBookingSystem.Helpers;
using CinemaTicketBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ActorsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Actor actor, IFormFile? imgFile)
        {
            if (!ModelState.IsValid) return View(actor);

            actor.Img = await FileHelper.SaveImageAsync(imgFile, _env.WebRootPath, "actors");

            _context.Add(actor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تمت إضافة الممثل بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Actor actor, IFormFile? imgFile)
        {
            if (id != actor.Id) return NotFound();
            if (!ModelState.IsValid) return View(actor);

            var existing = await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (existing == null) return NotFound();

            if (imgFile != null)
            {
                FileHelper.DeleteImage(existing.Img, _env.WebRootPath);
                actor.Img = await FileHelper.SaveImageAsync(imgFile, _env.WebRootPath, "actors");
            }
            else
            {
                actor.Img = existing.Img;
            }

            _context.Update(actor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم تعديل الممثل بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                FileHelper.DeleteImage(actor.Img, _env.WebRootPath);
                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الممثل بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
