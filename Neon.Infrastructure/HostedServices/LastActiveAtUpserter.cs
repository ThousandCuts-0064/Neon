using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neon.Application.Services.Systems;
using Neon.Infrastructure.Configurations;

namespace Neon.Infrastructure.HostedServices;

internal class LastActiveAtUpserter : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IOptionsMonitor<LastActiveAtUpserterConfiguration> _configuration;

    public LastActiveAtUpserter(
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<LastActiveAtUpserterConfiguration> configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await UpsertAsync();
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await UpsertAsync();
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_configuration.CurrentValue.DeltaSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await UpsertAsync();

            timer.Period = TimeSpan.FromSeconds(_configuration.CurrentValue.DeltaSeconds);
        }
    }

    private async Task UpsertAsync()
    {
        await using var serviceScope = _serviceScopeFactory.CreateAsyncScope();

        await serviceScope.ServiceProvider
            .GetRequiredService<ISystemService>()
            .UpsertLastActiveAtAsync();
    }
}