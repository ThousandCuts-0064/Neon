using Microsoft.AspNetCore.Mvc;

namespace Neon.Web.Controllers;

public class GameplayController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}