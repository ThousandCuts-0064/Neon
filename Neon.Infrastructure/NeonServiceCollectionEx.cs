using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Services;
using Neon.Infrastructure.Services;

namespace Neon.Infrastructure;

public static class NeonServiceCollectionEx
{
    public static IServiceCollection AddNeonInfrastructure(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthenticateService, AuthenticateService>()
            .AddScoped<IGameplayService, GameplayService>();
    }
}