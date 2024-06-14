using Microsoft.Extensions.DependencyInjection;
using Neon.Application;
using Neon.Domain;

namespace Neon.Infrastructure;

public static class NeonServiceCollectionEx
{
    public static IServiceCollection AddNeonApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<INeonDomain, NeonDomain>()
            .AddScoped<INeonApplication, NeonApplication>();
    }
}