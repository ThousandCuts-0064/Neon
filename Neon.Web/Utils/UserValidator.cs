using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neon.Data;
using Neon.Domain.Entities;

namespace Neon.Web.Utils;

public static class UserValidator
{
    public static async Task ValidatePrincipalAsync(CookieValidatePrincipalContext context)
    {
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();

        if (context.Principal is null || !int.TryParse(userManager.GetUserId(context.Principal), out var userId))
        {
            context.RejectPrincipal();
            return;
        }

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<NeonDbContext>();

        if (!await dbContext.Users.AnyAsync(x => x.Id == userId))
            context.RejectPrincipal();
    }
}
