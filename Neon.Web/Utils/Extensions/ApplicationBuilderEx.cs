using Microsoft.AspNetCore.Identity;
using Neon.Application.Services;

namespace Neon.Web.Utils.Extensions;

public static class ApplicationBuilderEx
{
    public static async Task EnsureRoles(this IApplicationBuilder app, IEnumerable<string> roles)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }

    public static async Task ClearUserConnections(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var gameplayService = scope.ServiceProvider.GetRequiredService<IGameplayService>();

        await gameplayService.ClearUserConnectionsAsync();
    }
}