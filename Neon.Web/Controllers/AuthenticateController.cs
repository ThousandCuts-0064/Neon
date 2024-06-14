using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neon.Application;
using Neon.Domain.Users;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

[AllowAnonymous]
public class AuthenticateController : NeonControllerBase
{
    private readonly SignInManager<User> _signInManager;

    public AuthenticateController(INeonApplication application, SignInManager<User> signInManager) : base(application)
    {
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Guest");
    }

    public IActionResult Guest()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Guest([FromForm] GuestModel model)
    {
        var guestResult = await Application.UserService.GuestAsync(model.Username);

        if (!guestResult.Succeeded)
            return View(model);

        await _signInManager.SignInAsync(new User { UserName = model.Username }, model.RememberMe);

        return RedirectToAction("Index", "Gameplay");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Login([FromForm] LoginModel model)
    {
        //return Application.UserService.Login(model.Username, model.Password, out var id) switch
        //{
        //    LoginResult.UsernameNotFound =>
        //        ViewWithError(model, nameof(model.Username), Resource.Error_Validation_UsernameNotFound),

        //    LoginResult.WrongPassword =>
        //        ViewWithError(model, nameof(model.Password), Resource.Error_Validation_WrongPassword),

        //    LoginResult.Success =>
        //        SignIn(id, model.Username, UserRole.Standard, model.RememberMe),

        //    _ => throw new InvalidOperationException()
        //};

        throw new NotImplementedException();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Register([FromForm] RegisterModel model)
    {
        //return Application.UserService.Register(model.Username, model.Password, out var id) switch
        //{
        //    RegisterResult.Success =>
        //        SignIn(id, model.Username, UserRole.Standard, model.RememberMe),

        //    RegisterResult.UsernameTaken =>
        //        ViewWithError(model, nameof(model.Username), Resource.Error_Validation_UsernameTaken),

        //    _ => throw new InvalidOperationException()
        //};

        throw new NotImplementedException();
    }

    private IActionResult SignIn(int id, string username, UserRole role, bool rememberMe)
    {
        return SignIn(new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()), new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role.ToString())
                ],
                CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                RedirectUri = Url.Action("Index", "Gameplay")
            });
    }

    private IActionResult ViewWithError(GuestModel model, string key, string message)
    {
        ModelState.AddModelError(key, message);

        return View(model);
    }
}