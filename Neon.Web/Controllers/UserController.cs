using Microsoft.AspNetCore.Mvc;
using Neon.Application.Services.Users;
using Neon.Web.Models;
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userService.FindAsync<UserModel>(User.GetId());

        return View(user);
    }
}