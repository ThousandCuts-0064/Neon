using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application.Repositories;
using Neon.Application.Services;
using Neon.Infrastructure.Configurations;
using Neon.Infrastructure.HostedServices;
using Neon.Infrastructure.Repositories;
using Neon.Infrastructure.Services;

namespace Neon.Infrastructure;

public static class NeonServiceCollectionEx
{
    public static IServiceCollection AddNeonInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<LastActiveAtUpserterConfiguration>(configuration.GetSection(""))
            .AddScoped<ISystemValueRepository, SystemValueRepository>()
            .AddScoped<ISystemValueService, SystemValueService>()
            .AddScoped<IAuthenticateService, AuthenticateService>()
            .AddScoped<IGameplayService, GameplayService>()
            .AddHostedService<LastActiveAtUpserter>();
    }
}