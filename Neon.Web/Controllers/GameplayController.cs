using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neon.Domain.Users;
using Neon.Web.Models;

namespace Neon.Web.Controllers;

//[Authorize]
public class GameplayController : Controller
{
    public IActionResult Index()
    {
        return View(new GameplayModel
        {
            User = new UserModel
            {
                Username = User.FindFirstValue(ClaimTypes.Name)!,
                Role = Enum.Parse<UserRole>(User.FindFirstValue(ClaimTypes.Role)!)
            }
        });
    }
}