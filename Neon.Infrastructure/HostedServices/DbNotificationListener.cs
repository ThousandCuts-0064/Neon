using System.Collections.Frozen;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.DbNotifications.Bases;
using Npgsql;

namespace Neon.Infrastructure.HostedServices;

internal class DbNotificationListener : BackgroundService
{
    private static readonly FrozenDictionary<string, Type> _dbNotificationTypes;
    private readonly AsyncServiceScope _asyncServiceScope;

    static DbNotificationListener()
    {
        _dbNotificationTypes = typeof(DbNotification).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsAssignableTo(typeof(DbNotification)))
            .ToFrozenDictionary(x => x.Name);
    }

    public DbNotificationListener(IServiceScopeFactory serviceScopeFactory)
    {
        _asyncServiceScope = serviceScopeFactory.CreateAsyncScope();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        await _asyncServiceScope.DisposeAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dbNotificationServcie = _asyncServiceScope.ServiceProvider.GetRequiredService<IDbNotificationService>();
        var dbContext = _asyncServiceScope.ServiceProvider.GetRequiredService<NeonDbContext>();
        var dbConnection = (NpgsqlConnection)dbContext.Database.GetDbConnection();

        dbConnection.Notification += (_, e) =>
        {
            if (!_dbNotificationTypes.TryGetValue(e.Channel, out var dbNotificationType))
                return;

            dbNotificationServcie.Notify(
                (DbNotification)JsonSerializer.Deserialize(e.Payload, dbNotificationType)!);
        };

        await dbConnection.OpenAsync(stoppingToken);

        await dbContext.ListenActiveConnectionToggleAsync();

        while (!stoppingToken.IsCancellationRequested)
            await dbConnection.WaitAsync(stoppingToken);
    }
}