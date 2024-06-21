using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Services;
using Neon.Domain.Users;
using Neon.Web.Hubs;
using Neon.Web.Models;
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Controllers;

public class GameplayController : Controller
{
    private readonly IHubContext<GameplayHub, IGameplayHubClient> _gameplayHub;
    private readonly IGameplayService _gameplayService;

    public GameplayController(IHubContext<GameplayHub, IGameplayHubClient> gameplayHub, IGameplayService gameplayService)
    {
        _gameplayHub = gameplayHub;
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
}