using System.Collections.Frozen;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neon.Application.Services.Notifications;
using Neon.Data;
using Neon.Domain.Notifications;
using Neon.Domain.Notifications.Bases;
using Npgsql;

namespace Neon.Infrastructure.HostedServices;

internal class DbNotificationListener : BackgroundService
{
    private static readonly FrozenDictionary<string, Type> _dbNotificationTypes;
    private readonly AsyncServiceScope _asyncServiceScope;

    static DbNotificationListener()
    {
        _dbNotificationTypes = typeof(Notification).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && x.IsAssignableTo(typeof(Notification)))
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
        var notificationServcie = _asyncServiceScope.ServiceProvider.GetRequiredService<INotificationService>();
        var dbContext = _asyncServiceScope.ServiceProvider.GetRequiredService<NeonDbContext>();
        var dbConnection = (NpgsqlConnection)dbContext.Database.GetDbConnection();

        dbConnection.Notification += (_, e) =>
        {
            if (!_dbNotificationTypes.TryGetValue(e.Channel, out var dbNotificationType))
                return;

            notificationServcie.Notify(
                (Notification)JsonSerializer.Deserialize(e.Payload, dbNotificationType)!);
        };

        await dbConnection.OpenAsync(stoppingToken);

        await dbContext.ListenAsync<UserConnectionToggled>();

        await dbContext.ListenAsync<FriendRequestSent>();
        await dbContext.ListenAsync<FriendRequestAccepted>();
        await dbContext.ListenAsync<FriendRequestDeclined>();
        await dbContext.ListenAsync<FriendRequestCanceled>();

        await dbContext.ListenAsync<TradeRequestSent>();
        await dbContext.ListenAsync<TradeRequestAccepted>();
        await dbContext.ListenAsync<TradeRequestDeclined>();
        await dbContext.ListenAsync<TradeRequestCanceled>();

        await dbContext.ListenAsync<DuelRequestSent>();
        await dbContext.ListenAsync<DuelRequestAccepted>();
        await dbContext.ListenAsync<DuelRequestDeclined>();
        await dbContext.ListenAsync<DuelRequestCanceled>();

        while (!stoppingToken.IsCancellationRequested)
            await dbConnection.WaitAsync(stoppingToken);
    }
}