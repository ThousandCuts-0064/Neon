using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Neon.Infrastructure;

public static class ApplicationBuilderEx
{
    public static IApplicationBuilder UseNeonInfrastructure(this IApplicationBuilder app)
    {
        return app.UseSerilogRequestLogging();
    }
}