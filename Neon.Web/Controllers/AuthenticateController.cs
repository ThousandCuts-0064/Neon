using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neon.Application.Services.Authentications;
using Neon.Domain.Enums;
using Neon.Web.Models;
using Neon.Web.Resources;

namespace Neon.Web.Controllers;

[AllowAnonymous]
public class AuthenticateController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticateController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
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
        var result = await _authenticationService.GuestAsync(model.Username, model.RememberMe);

        if (result == RegisterResult.Success)
            return RedirectToAction("Index", "Lobby");

        if (result == RegisterResult.Error)
        {
            ModelState.AddModelError("", Resource.Error_Generic_UnexpectedError);

            return View(model);
        }

        var usernameError = result switch
        {
            RegisterResult.UsernameTaken => Resource.Error_Authenticate_UsernameTaken,
            RegisterResult.UsernameInvalidCharacters => Resource.Error_Authenticate_UsernameInvalidCharacters,
            _ => throw new UnreachableException()
        };

        ModelState.AddModelError(nameof(model.Username), usernameError);

        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginModel model)
    {
        var result = await _authenticationService.LoginAsync(model.Username, model.Password, model.RememberMe);

        if (result == LoginResult.Success)
            return RedirectToAction("Index", "Lobby");

        if (result == LoginResult.Error)
        {
            ModelState.AddModelError("", Resource.Error_Generic_UnexpectedError);

            return View(model);
        }

        var (errorKey, errorMessage) = result switch
        {
            LoginResult.CannotLogInGuest => (nameof(model.Username), Resource.Error_Authenticate_CannotLogInGuest),
            LoginResult.UsernameNotFound => (nameof(model.Username), Resource.Error_Authenticate_UsernameNotFound),
            LoginResult.WrongPassword => (nameof(model.Password), Resource.Error_Authenticate_WrongPassword),
            _ => throw new UnreachableException()
        };

        ModelState.AddModelError(errorKey, errorMessage);

        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegisterModel model)
    {
        var result = await _authenticationService.RegisterAsync(model.Username, model.Password, model.RememberMe);

        if (result == RegisterResult.Success)
            return RedirectToAction("Index", "Lobby");

        if (result == RegisterResult.Error)
        {
            ModelState.AddModelError("", Resource.Error_Generic_UnexpectedError);

            return View(model);
        }

        var usernameError = result switch
        {
            RegisterResult.UsernameTaken => Resource.Error_Authenticate_UsernameTaken,
            RegisterResult.UsernameInvalidCharacters => Resource.Error_Authenticate_UsernameInvalidCharacters,
            RegisterResult.WeakPassword => Resource.Error_Authenticate_WeakPassword,
            _ => throw new UnreachableException()
        };

        ModelState.AddModelError(nameof(model.Username), usernameError);

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _authenticationService.LogoutAsync();

        return RedirectToAction("Index");
    }
}