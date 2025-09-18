using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroanZone.Data;
using GroanZone.Filters;
using GroanZone.Models;

namespace GroanZone.Controllers
{
    [SessionAuth]
    public class RatingsController : Controller
    {
        private readonly AppDbContext _db;
        public RatingsController(AppDbContext db) => _db = db;

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Rate([FromForm]int jokeId, [FromForm(Name ="value")] int ratingValue)
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            if (ratingValue < 1 || ratingValue > 4)
            {
                TempData["Error"] = "Rating must be 1-4.";
                return RedirectToAction("Details", "Jokes", new { id = jokeId });
            }

            _db.ChangeTracker.Clear();

            var exists = await _db.Ratings.AsNoTracking().AnyAsync(j => j.JokeId == jokeId && j.UserId == userId);
            if (exists)
            {
                TempData["Error"] = "You have already rated this joke.";
                return RedirectToAction("Details", "Jokes", new { id = jokeId });
            }

            _db.Ratings.Add(new Rating
            {
                JokeId = jokeId,
                UserId = userId,
                Value = ratingValue,
            });
            
            await _db.SaveChangesAsync();

            TempData["Success"] = "Thanks for your rating! It has been recorded.";
            return RedirectToAction("Details", "Jokes", new { id = jokeId });
        }
    }
}