﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Services;
using Neon.Domain.Enums;
using Neon.Infrastructure.MIddlewares;

namespace Neon.Infrastructure;

public static class ApplicationBuilderEx
{
    public static async Task UseNeonInfrastructure(
        this IApplicationBuilder app,
        Func<IServiceProvider, ValueTask>? onSetup)
    {
        app.UseMiddleware<ChallengeDeletedUsers>();

        await using var serviceScope = app.ApplicationServices.CreateAsyncScope();

        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var role in Enum.GetNames<UserRole>())
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        await serviceScope.ServiceProvider
            .GetRequiredService<IGameplayService>()
            .ClearUserConnectionsAsync();

        if (onSetup is not null)
            await onSetup(serviceScope.ServiceProvider);
    }
}