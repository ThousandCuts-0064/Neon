using Microsoft.Extensions.Hosting;
using Serilog;

namespace Neon.Infrastructure;

public static class HostBuilderEx
{
    public static IHostBuilder UseNeonInfrastructure(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }
}