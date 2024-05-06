using Microsoft.AspNetCore.Mvc;

namespace Neon.Web.Controllers;

public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
