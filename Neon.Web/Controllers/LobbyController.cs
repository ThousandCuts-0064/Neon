using Microsoft.AspNetCore.Mvc;
using Neon.Application.Extensions;
using Neon.Application.Services.Users;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

public class LobbyController : Controller
{
    private readonly IUserService _userService;

    public LobbyController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userService.FindAsync<UserModel>(User.GetId());

        return View(new LobbyModel
        {
            User = user
        });
    }
}