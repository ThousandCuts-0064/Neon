using Microsoft.AspNetCore.Mvc;
using Neon.Application.Extensions;
using Neon.Application.Services.Users;
using Neon.Web.Models;
using Neon.Web.Utils.TagHelpers;

namespace Neon.Web.Controllers;

public class LobbyController : Controller
{
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public LobbyController(IUserService userService, IWebHostEnvironment webHostEnvironment)
    {
        _userService = userService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetId();
        var user = await _userService.FindAsync<UserModel>(userId);
        var activeUsers = await _userService.FindOtherActiveUsersAsync<ActiveUserModel>(userId);
        var friends = await _userService.FindFriendsAsync<UserModel>(userId);
        var incomingUserRequests = await _userService.FindIncomingUserRequests<IncomingUserRequestModel>(userId);
        var outgoingUserRequests = await _userService.FindOutgoingUserRequests<OutgoingUserRequestModel>(userId);

        return View(new LobbyModel
        {
            User = user,
            ActiveUsers = activeUsers,
            Friends = friends,
            IncomingUserRequests = incomingUserRequests,
            OutgoingUserRequests = outgoingUserRequests
        });
    }
}