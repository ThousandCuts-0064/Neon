using Microsoft.AspNetCore.Mvc;
using Neon.Application.Services;
using Neon.Web.Models;
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Controllers;

public class UserController : Controller
{
    private readonly IGameplayService _gameplayService;

    public UserController(IGameplayService gameplayService)
    {
        _gameplayService = gameplayService;
    }

    public IActionResult Index()
    {
        var user = _gameplayService.FindUserById(User.GetId());

        return View(new UserModel
        {
            Username = user.UserName
        });
    }
}
