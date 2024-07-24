using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Repositories;
using Neon.Application.Services;
using Neon.Infrastructure.Configurations;
using Neon.Infrastructure.Configurations.Bases;
using Neon.Infrastructure.HostedServices;
using Neon.Infrastructure.MIddlewares;
using Neon.Infrastructure.Repositories;
using Neon.Infrastructure.Services;

namespace Neon.Infrastructure;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddNeonInfrastructure(
        this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        return services
            .ConfigureBindable<LastActiveAtUpserterConfiguration>(configuration)
            .AddHostedService<LastActiveAtUpserter>()
            .AddHostedService<DbNotificationListener>()
            .AddSingleton<IDbNotificationService, DbNotificationService>()
            .AddScoped<ChallengeDeletedUsers>()
            .AddScoped<ISystemValueRepository, SystemValueRepository>()
            .AddScoped<ISystemValueService, SystemValueService>()
            .AddScoped<IAuthenticateService, AuthenticateService>()
            .AddScoped<IGameplayService, GameplayService>();
    }

    private static IServiceCollection ConfigureBindable<T>(
        this IServiceCollection services,
        IConfigurationRoot configuration)
        where T : class, IConfigurationBindable
    {
        return services.Configure<T>(configuration.GetSection(T.Key));
    }
}