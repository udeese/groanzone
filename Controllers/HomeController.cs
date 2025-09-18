using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GroanZone.Models;

namespace GroanZone.Controllers;

public class HomeController : Controller
{
    public IActionResult Error() => View();
    public IActionResult HttpError(int code)
    {
        if (code == 404) return View("NotFound");
        if (code == 401) return View("Unauthorized");
        if (code == 403) return View("Forbidden");
        return View("Error");
    }
}