using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroanZone.Data;
using GroanZone.Filters;
using GroanZone.Models;
using GroanZone.ViewModels;

namespace GroanZone.Controllers
{
    [SessionAuth]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _db;
        public ProfileController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId")!.Value;

            var added = await _db.Jokes
                .AsNoTracking()
                .CountAsync(j => j.UserId == userId);

            var rated = await _db.Ratings
                .AsNoTracking()
                .CountAsync(r => r.UserId == userId);

            var recentJokes = await _db.Jokes
                .AsNoTracking()
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.CreatedAt)
                .Take(5)
                .ToListAsync();
            
            ViewData["AddedCount"] = added;
            ViewData["RatedCount"] = rated;
            return View(recentJokes);
        }
    }
}