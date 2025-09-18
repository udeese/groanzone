using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroanZone.Data;
using GroanZone.Filters;
using GroanZone.Models;
using GroanZone.ViewModels;

namespace GroanZone.Controllers
{
    [SessionAuth]
    public class JokesController : Controller
    {
        private readonly AppDbContext _db;
        public JokesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var jokes = await _db.Jokes
                .AsNoTracking()
                .Include(j => j.Author)
                .Include(j => j.Ratings)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return View(jokes);
        }

        [HttpGet]
        public IActionResult Create() => View(new JokeFormVM());

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(JokeFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var joke = new Joke
            {
                Setup = vm.Setup,
                Punchline = vm.Punchline,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Jokes.Add(joke);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = joke.JokeId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var joke = await _db.Jokes
                .AsNoTracking()
                .Include(j => j.Author)
                .Include(j => j.Ratings)
                .FirstOrDefaultAsync(j => j.JokeId == id);

            if (joke is null) return RedirectToAction("HttpError", "Home", new { code = 404 });

            var avg = joke.Ratings.Any() ? Math.Round(joke.Ratings.Average(r => r.Value), 2) : 0.0;
            var hasUserRated = userId.HasValue && joke.Ratings.Any(r => r.UserId == userId.Value);
            var existing = hasUserRated ? joke.Ratings.First(r => r.UserId == userId!.Value).Value : (int?)null;

            var vm = new JokeDetailsVM
            {
                Joke = joke,
                AverageRating = avg,
                HasUserRated = hasUserRated,
                ExistingUserRating = existing
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var joke = await _db.Jokes.FirstOrDefaultAsync(j => j.JokeId == id);
            if (joke is null) return RedirectToAction("HttpError", "Home", new { code = 404 });
            if (joke.UserId != userId) return Forbid();

            var vm = new JokeFormVM
            {
                JokeId = joke.JokeId,
                Setup = joke.Setup,
                Punchline = joke.Punchline
            };
            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, JokeFormVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var joke = await _db.Jokes.FirstOrDefaultAsync(j => j.JokeId == id);
            if (joke is null) return RedirectToAction("HttpError", "Home", new { code = 404 });
            if (joke.UserId != userId) return Forbid();

            joke.Setup = vm.Setup;
            joke.Punchline = vm.Punchline;
            joke.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = joke.JokeId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var joke = await _db.Jokes.FirstOrDefaultAsync(j => j.JokeId == id);
            if (joke is null) return RedirectToAction("HttpError", "Home", new { code = 404 });
            if (joke.UserId != userId) return Forbid();
            return View(joke);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var joke = await _db.Jokes.FirstOrDefaultAsync(j => j.JokeId == id);
            if (joke is null) return RedirectToAction("HttpError", "Home", new { code = 404 });
            if (joke.UserId != userId) return Forbid();

            _db.Jokes.Remove(joke);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

}