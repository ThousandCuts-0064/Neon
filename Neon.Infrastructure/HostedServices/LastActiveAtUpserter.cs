using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neon.Application.Services;

namespace Neon.Infrastructure.HostedServices;

internal class LastActiveAtUpserter : BackgroundService
{
    private readonly IServiceScope _serviceScope;
    private readonly ISystemValueService _systemValueService;
    private readonly IConfiguration _configuration;

    public LastActiveAtUpserter(IServiceProvider services)
    {
        _serviceScope = services.CreateScope();

        _systemValueService = _serviceScope.ServiceProvider.GetRequiredService<ISystemValueService>();
        _configuration = _serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await Upsert();
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await Upsert();
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
        _serviceScope.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var seconds = int.TryParse(_configuration["UpdateLastActiveAtDeltaSeconds"], out var parsedSeconds)
            ? parsedSeconds
            : 10;

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(seconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
            await Upsert();
    }

    private Task Upsert() => _systemValueService.UpsertLastActiveAtAsync();
}