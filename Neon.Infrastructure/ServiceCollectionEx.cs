using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application;
using Neon.Data;
using Neon.Infrastructure.Configurations;
using Neon.Infrastructure.Configurations.Bases;
using Neon.Infrastructure.HostedServices;
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
            .AddDbContext<INeonDbContext, NeonDbContext>();
    }

    private static IServiceCollection ConfigureBindable<T>(
        this IServiceCollection services,
        IConfigurationRoot configuration)
        where T : class, IConfigurationBindable
    {
        return services.Configure<T>(configuration.GetSection(T.Key));
    }
}