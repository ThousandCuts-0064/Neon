using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neon.Application.Services;
using Neon.Infrastructure.Configurations;

namespace Neon.Infrastructure.HostedServices;

internal class LastActiveAtUpserter : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IOptionsMonitor<LastActiveAtUpserterConfiguration> _configuration;

    public LastActiveAtUpserter(
        IServiceProvider services,
        IOptionsMonitor<LastActiveAtUpserterConfiguration> configuration)
    {
        _services = services;
        _configuration = configuration;
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_configuration.CurrentValue.DeltaSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await Upsert();

            timer.Period = TimeSpan.FromSeconds(_configuration.CurrentValue.DeltaSeconds);
        }
    }

    private async Task Upsert()
    {
        await using var serviceScope = _services.CreateAsyncScope();

        await serviceScope.ServiceProvider
            .GetRequiredService<ISystemValueService>()
            .UpsertLastActiveAtAsync();
    }
}