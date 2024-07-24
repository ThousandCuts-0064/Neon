using Neon.Application.Services;
using Neon.Domain.DbNotifications.Bases;

namespace Neon.Infrastructure.Services;

internal class DbNotificationService : IDbNotificationService
{
    private readonly Dictionary<Type, Notifier> _notifiers = [];

    public void Notify(DbNotification dbNotification)
    {
        if (_notifiers.TryGetValue(dbNotification.GetType(), out var handler))
            handler.Notify(dbNotification);
    }

    public void Listen<T>(Action<T> handler) where T : DbNotification
    {
        if (_notifiers.TryGetValue(typeof(T), out var notifier))
        {
            notifier.Listen(handler);

            return;
        }

        notifier = new Notifier<T>();
        notifier.Listen(handler);

        _notifiers.Add(typeof(T), notifier);
    }

    public void Unlisten<T>(Action<T> handler) where T : DbNotification
    {
        if (_notifiers.TryGetValue(typeof(T), out var notifier))
            notifier.Unlisten(handler);
    }

    private abstract class Notifier
    {
        public abstract void Notify(DbNotification dbNotification);
        public abstract void Listen<T>(Action<T> handler) where T : DbNotification;
        public abstract void Unlisten<T>(Action<T> handler) where T : DbNotification;
    }

    private class Notifier<T> : Notifier where T : DbNotification
    {
        private Action<T>? _onNotify;

        public override void Notify(DbNotification dbNotification) => _onNotify?.Invoke((T)dbNotification);
        public override void Listen<T1>(Action<T1> handler) => _onNotify += (Action<T>)handler;
        public override void Unlisten<T1>(Action<T1> handler) => _onNotify -= (Action<T>)handler;
    }
}