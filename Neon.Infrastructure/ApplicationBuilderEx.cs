using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Services.Systems;
using Neon.Application.Services.Users;

namespace Neon.Infrastructure;

public static class ApplicationBuilderEx
{
    public static async Task UseNeonInfrastructureAsync(
        this IApplicationBuilder app,
        Func<IServiceProvider, ValueTask>? onSetup)
    {
        await using var serviceScope = app.ApplicationServices.CreateAsyncScope();

        var lastActiveAt = await serviceScope.ServiceProvider
            .GetRequiredService<ISystemService>()
            .FindLastActiveAtAsync();

        if (lastActiveAt is not null)
        {
            await serviceScope.ServiceProvider
                .GetRequiredService<IUserService>()
                .SetAllInactiveAsync(lastActiveAt.Value);
        }

        if (onSetup is not null)
            await onSetup(serviceScope.ServiceProvider);
    }
}