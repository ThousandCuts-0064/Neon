using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neon.Application;
using Neon.Domain.Users;
using Neon.Web.Models;
using Neon.Web.Resources;
using AuthenticateResult = Neon.Application.Services.Users.AuthenticateResult;

namespace Neon.Web.Controllers;

[AllowAnonymous]
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
    public IActionResult Guest([FromForm] LoginModel model)
    {
        if (string.IsNullOrEmpty(model.Secret))
            return HandleNewGuest(model);

        return Application.UserService.Authenticate(model.Username, model.Secret) switch
        {
            AuthenticateResult.UsernameNotFound => HandleNewGuest(model),
            AuthenticateResult.SecretMismatch => ViewUsernameTaken(model),
            AuthenticateResult.Success => RedirectToAction("Index", "Gameplay"),
            _ => throw new UnreachableException()
        };
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    private IActionResult SignIn(int id, string username, UserRole role)
    {
        return SignIn(
            new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role.ToString())
                ],
                CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = Url.Action("Index", "Gameplay")
            });
    }

    private IActionResult HandleNewGuest(LoginModel model)
    {
        return Application.UserService.CreateGuest(model.Username, out var id)
            ? SignIn(id, model.Username, UserRole.Guest)
            : ViewUsernameTaken(model);
    }

    private IActionResult ViewUsernameTaken(LoginModel model)
    {
        ModelState.AddModelError(nameof(model.Username), Resource.Error_Validation_UsernameTaken);

        return View(model);
    }
}