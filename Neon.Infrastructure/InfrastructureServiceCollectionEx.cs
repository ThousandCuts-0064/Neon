using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application;
using Neon.Data.Core;
using Neon.Data.Identity;

namespace Neon.Infrastructure;

public static class InfrastructureServiceCollectionEx
{
    public static IServiceCollection AddNeonIdentity(this IServiceCollection services)
    {
        services
            .AddDbContext<NeonIdentityDbContext>((x, _) => x
                .GetRequiredService<IConfiguration>()
                .GetConnectionString("Identity"))
            .AddIdentityCore<NeonUser>()
            .AddEntityFrameworkStores<NeonIdentityDbContext>();

        return services;
    }

    public static IServiceCollection AddNeonApplication(this IServiceCollection services)
    {
        services
            .AddDbContext<NeonDbContext>((x, _) => x
                .GetRequiredService<IConfiguration>()
                .GetConnectionString("Default"))
            .AddScoped<INeonApplication>(x =>
                new NeonApplication(new NeonDomain(x.GetRequiredService<NeonDbContext>())));

        return services;
    }
}