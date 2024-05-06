using Microsoft.AspNetCore.Mvc;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

public class AuthenticateController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Guest");
    }

    public IActionResult Guest()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Guest([FromBody] GuestModel guest)
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
}
