using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroanZone.Data;
using GroanZone.Models;
using GroanZone.ViewModels;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GroanZone.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        public AuthController(AppDbContext db) => _db = db;

        [HttpGet("/")]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Register() => View(new RegisterVM());

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _db.Users.AsNoTracking().AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "Email already in use.");
                return View(vm);
            }

            var user = new User
            {
                Username = vm.Username,
                Email = vm.Email,
                PasswordHash = BCryptNet.HashPassword(vm.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Jokes");
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginVM());

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (user is null || !BCryptNet.Verify(vm.Password, user.PasswordHash))
            {
                ModelState.AddModelError("Password", "Invalid credentials.");
                return View(vm);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Jokes");
        }

        [HttpGet]
        public IActionResult LogoutConfirm() => View();

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}