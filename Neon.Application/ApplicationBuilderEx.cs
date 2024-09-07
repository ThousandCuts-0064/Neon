using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Services.Systems;
using Neon.Application.Services.Users;

namespace Neon.Application;

public static class ApplicationBuilderEx
{
    public static async Task InitializeNeonAsync(this IApplicationBuilder app)
    {
        await using var serviceScope = app.ApplicationServices.CreateAsyncScope();

        await InitializeNeonCoreAsync(serviceScope.ServiceProvider);
    }

    public static async Task InitializeNeonAsync<TInitializer>(this IApplicationBuilder app)
        where TInitializer : IInitializer
    {
        await using var serviceScope = app.ApplicationServices.CreateAsyncScope();

        await InitializeNeonCoreAsync(serviceScope.ServiceProvider);

        await ActivatorUtilities
            .GetServiceOrCreateInstance<TInitializer>(serviceScope.ServiceProvider)
            .RunAsync();
    }

    private static async Task InitializeNeonCoreAsync(IServiceProvider serviceProvider)
    {
        var lastActiveAt = await serviceProvider
            .GetRequiredService<ISystemService>()
            .FindLastActiveAtAsync();

        if (lastActiveAt is not null)
        {
            await serviceProvider
                .GetRequiredService<IUserService>()
                .SetAllInactiveAsync(lastActiveAt.Value);
        }
    }
}