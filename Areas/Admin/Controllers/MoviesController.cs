using CinemaTicketBookingSystem.Data;
using CinemaTicketBookingSystem.Helpers;
using CinemaTicketBookingSystem.Models;
using CinemaTicketBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MoviesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.SubImages)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();
            return View(movie);
        }

        private async Task PopulateDropdowns(MovieFormViewModel vm)
        {
            vm.Categories = await _context.Categories.ToListAsync();
            vm.Cinemas = await _context.Cinemas.ToListAsync();
            vm.Actors = await _context.Actors.ToListAsync();
        }

        public async Task<IActionResult> Create()
        {
            var vm = new MovieFormViewModel { DateTime = DateTime.Now };
            await PopulateDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vm);
                return View(vm);
            }

            var movie = new Movie
            {
                Name = vm.Name,
                Des = vm.Des,
                Price = vm.Price,
                Status = vm.Status,
                DateTime = vm.DateTime,
                CategoryId = vm.CategoryId,
                CinemaId = vm.CinemaId
            };

            movie.MainImg = await FileHelper.SaveImageAsync(vm.MainImgFile, _env.WebRootPath, "movies");

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // حفظ الصور الفرعية
            if (vm.SubImageFiles != null)
            {
                foreach (var file in vm.SubImageFiles)
                {
                    var path = await FileHelper.SaveImageAsync(file, _env.WebRootPath, "movies");
                    if (path != null)
                        _context.MovieImages.Add(new MovieImage { MovieId = movie.Id, ImagePath = path });
                }
            }

            // ربط الممثلين المختارين
            foreach (var actorId in vm.SelectedActorIds)
            {
                _context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actorId });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "تمت إضافة الفيلم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.SubImages)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var vm = new MovieFormViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Des = movie.Des,
                Price = movie.Price,
                Status = movie.Status,
                DateTime = movie.DateTime,
                CategoryId = movie.CategoryId,
                CinemaId = movie.CinemaId,
                ExistingMainImg = movie.MainImg,
                ExistingSubImages = movie.SubImages.ToList(),
                SelectedActorIds = movie.MovieActors.Select(ma => ma.ActorId).ToList()
            };

            await PopulateDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vm);
                return View(vm);
            }

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            movie.Name = vm.Name;
            movie.Des = vm.Des;
            movie.Price = vm.Price;
            movie.Status = vm.Status;
            movie.DateTime = vm.DateTime;
            movie.CategoryId = vm.CategoryId;
            movie.CinemaId = vm.CinemaId;

            if (vm.MainImgFile != null)
            {
                FileHelper.DeleteImage(movie.MainImg, _env.WebRootPath);
                movie.MainImg = await FileHelper.SaveImageAsync(vm.MainImgFile, _env.WebRootPath, "movies");
            }

            // إضافة صور فرعية جديدة إن وجدت (الصور القديمة تبقى، ويمكن حذفها من صفحة التفاصيل)
            if (vm.SubImageFiles != null)
            {
                foreach (var file in vm.SubImageFiles)
                {
                    var path = await FileHelper.SaveImageAsync(file, _env.WebRootPath, "movies");
                    if (path != null)
                        _context.MovieImages.Add(new MovieImage { MovieId = movie.Id, ImagePath = path });
                }
            }

            // تحديث قائمة الممثلين
            _context.MovieActors.RemoveRange(movie.MovieActors);
            foreach (var actorId in vm.SelectedActorIds)
            {
                _context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actorId });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "تم تعديل الفيلم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.SubImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie != null)
            {
                FileHelper.DeleteImage(movie.MainImg, _env.WebRootPath);
                foreach (var img in movie.SubImages)
                    FileHelper.DeleteImage(img.ImagePath, _env.WebRootPath);

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الفيلم بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubImage(int imageId, int movieId)
        {
            var img = await _context.MovieImages.FindAsync(imageId);
            if (img != null)
            {
                FileHelper.DeleteImage(img.ImagePath, _env.WebRootPath);
                _context.MovieImages.Remove(img);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Edit), new { id = movieId });
        }
    }
}
