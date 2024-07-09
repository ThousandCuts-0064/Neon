﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neon.Application.Services;
using Neon.Domain.Enums;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

[AllowAnonymous]
public class AuthenticateController : Controller
{
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
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
        var result = await _authenticateService.GuestAsync(model.Username, model.RememberMe);

        if (result == RegisterResult.Success)
            return RedirectToAction("Index", "Gameplay");

        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginModel model)
    {
        var result = await _authenticateService.LoginAsync(model.Username, model.Password, model.RememberMe);

        if (result == LoginResult.Success)
            return RedirectToAction("Index", "Gameplay");

        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegisterModel model)
    {
        var result = await _authenticateService.RegisterAsync(model.Username, model.Password, model.RememberMe);

        if (result == RegisterResult.Success)
            return RedirectToAction("Index", "Gameplay");

        return View(model);
    }
}