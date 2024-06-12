using Microsoft.Extensions.DependencyInjection;
using Neon.Application;
using Neon.Data;

namespace Neon.Infrastructure;

public static class InfrastructureServiceCollectionEx
{
    public static IServiceCollection AddNeonApplication(this IServiceCollection services)
    {
        services.AddScoped<INeonApplication>(x =>
            new NeonApplication(new NeonDomain(x.GetRequiredService<NeonDbContext>())));

        return services;
    }
}