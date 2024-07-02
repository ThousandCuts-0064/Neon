using Microsoft.AspNetCore.Mvc;
using Neon.Application.Services;
using Neon.Web.Models;
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Controllers;

public class GameplayController : Controller
{
    private readonly IGameplayService _gameplayService;

    public GameplayController(IGameplayService gameplayService)
    {
        _gameplayService = gameplayService;
    }

    public IActionResult Index()
    {
        var opponents = _gameplayService.ActiveUsers
            .Where(x => x.UserName != User.GetUsername())
            .Select(x => new OpponentModel
            {
                Username = x.UserName
            })
            .ToList();

        return View(new GameplayModel
        {
            Opponents = opponents
        });
    }

    public IActionResult AlreadyActive()
    {
        return View();
    }
}