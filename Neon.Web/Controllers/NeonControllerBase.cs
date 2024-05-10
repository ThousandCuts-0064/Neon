using Microsoft.AspNetCore.Mvc;
using Neon.Application;

namespace Neon.Web.Controllers;

public abstract class NeonControllerBase : Controller
{
    protected INeonApplication Application { get; }

    protected NeonControllerBase(INeonApplication application)
    {
        Application = application;
    }
}