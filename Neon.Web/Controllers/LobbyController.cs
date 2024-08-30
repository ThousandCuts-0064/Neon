using Microsoft.AspNetCore.Mvc;
using Neon.Application.Extensions;
using Neon.Application.Services.Lobbies;
using Neon.Application.Services.Users;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

public class LobbyController : Controller
{
    private readonly ILobbyService _lobbyService;
    private readonly IUserService _userService;

    public LobbyController(ILobbyService lobbyService, IUserService userService)
    {
        _lobbyService = lobbyService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetId();
        var user = await _userService.FindAsync<UserModel>(userId);
        var activeUsers = await _lobbyService.FindActiveUsersAsync<ActiveUserModel>(userId);

        return View(new LobbyModel
        {
            User = user,
            ActiveUsers = activeUsers
        });
    }
}