using Microsoft.AspNetCore.Mvc;
using Neon.Application;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

public class AuthenticateController : NeonControllerBase
{
    public AuthenticateController(INeonApplication application) : base(application) { }

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
        if (!Application.UserRepository.ContainsUsername(guest.Username))
        {
            Application.UserRepository.Add(new GuestModel
            {
                guest.Username
            });
        }

        return RedirectToAction("Index", "Gameplay");
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