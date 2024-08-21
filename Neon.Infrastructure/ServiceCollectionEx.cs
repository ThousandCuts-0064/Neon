using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neon.Infrastructure.Configurations;
using Neon.Infrastructure.Configurations.Bases;
using Neon.Infrastructure.HostedServices;
using Neon.Infrastructure.MIddlewares;

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
            .AddScoped<ChallengeDeletedUsers>();
    }

    private static IServiceCollection ConfigureBindable<T>(
        this IServiceCollection services,
        IConfigurationRoot configuration)
        where T : class, IConfigurationBindable
    {
        return services.Configure<T>(configuration.GetSection(T.Key));
    }
}