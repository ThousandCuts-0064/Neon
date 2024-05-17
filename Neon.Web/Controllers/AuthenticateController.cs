using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Neon.Application;
using Neon.Web.Requests;

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
    public IStatusCodeActionResult Guest([FromForm] AuthenticateGuestRequest request)
    {
        var isAuthenticated = !string.IsNullOrEmpty(request.Secret) &&
            Application.UserService.AuthenticateGuest(request.Username, request.Secret);

        if (!isAuthenticated)
            return Ok();

        if (Application.UserService.CreateGuest(request.Username, out var secret))
            return CreatedAtAction("Guest", secret);

        return Conflict();
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