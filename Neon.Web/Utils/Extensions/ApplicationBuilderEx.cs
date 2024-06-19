using Microsoft.AspNetCore.Identity;

namespace Neon.Web.Utils.Extensions;

public static class ApplicationBuilderEx
{
    public static async Task<IApplicationBuilder> EnsureRoles(this IApplicationBuilder app, IEnumerable<string> roles)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        return app;
    }
}