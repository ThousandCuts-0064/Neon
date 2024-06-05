using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neon.Application;
using Neon.Data;

namespace Neon.Infrastructure;

public static class InfrastructureServiceCollectionEx
{
    public static IServiceCollection AddNeonApplication(this IServiceCollection services)
    {
        services.AddSingleton<INeonApplication>(x =>
            new NeonApplication(new NeonDomain(new NeonDbContext(x.GetRequiredService<IConfiguration>().GetConnectionString("Default")))));

        return services;
    }
}