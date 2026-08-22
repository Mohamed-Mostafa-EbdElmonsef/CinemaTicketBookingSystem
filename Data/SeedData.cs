using CinemaTicketBookingSystem.Models;

namespace CinemaTicketBookingSystem.Data
{
    // بيانات أولية بسيطة حتى تظهر الواجهة العامة بشكل جيد من أول تشغيل
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Categories.Any()) return; // البيانات موجودة بالفعل

            var categories = new List<Category>
            {
                new Category { Name = "أكشن" },
                new Category { Name = "دراما" },
                new Category { Name = "كوميدي" },
                new Category { Name = "رعب" },
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var cinemas = new List<Cinema>
            {
                new Cinema { Name = "سينما سيتي ستارز", Address = "القاهرة - مدينة نصر" },
                new Cinema { Name = "سينما مول العرب", Address = "الجيزة - 6 أكتوبر" },
            };
            context.Cinemas.AddRange(cinemas);
            context.SaveChanges();

            var actors = new List<Actor>
            {
                new Actor { Name = "أحمد عز" },
                new Actor { Name = "منى زكي" },
                new Actor { Name = "كريم عبد العزيز" },
            };
            context.Actors.AddRange(actors);
            context.SaveChanges();

            var movie = new Movie
            {
                Name = "فيلم تجريبي",
                Des = "هذا وصف تجريبي لفيلم تم إضافته تلقائياً عند أول تشغيل للمشروع.",
                Price = 100,
                Status = MovieStatus.NowShowing,
                DateTime = DateTime.Now.AddDays(1),
                CategoryId = categories[0].Id,
                CinemaId = cinemas[0].Id
            };
            context.Movies.Add(movie);
            context.SaveChanges();

            context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actors[0].Id });
            context.SaveChanges();
        }
    }
}
