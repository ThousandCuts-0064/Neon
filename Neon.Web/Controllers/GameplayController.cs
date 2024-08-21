using Microsoft.AspNetCore.Mvc;
using Neon.Application.Services.Gameplays;
using Neon.Application.Services.Users;
using Neon.Web.Models;
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Controllers;

public class GameplayController : Controller
{
    private readonly IGameplayService _gameplayService;
    private readonly IUserService _userService;

    public GameplayController(IGameplayService gameplayService, IUserService userService)
    {
        _gameplayService = gameplayService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetId();
        var user = await _userService.FindAsync<UserModel>(userId);
        var opponvents = await _gameplayService.FindOpponentsAsync<OpponentModel>(userId);

        return View(new GameplayModel
        {
            User = user,
            Opponents = opponvents
        });
    }
}