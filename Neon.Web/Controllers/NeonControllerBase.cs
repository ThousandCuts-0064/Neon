using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Neon.Web.Controllers;

public abstract class NeonControllerBase : Controller
{
    [Inject]
    public object A { get; set; }
}
