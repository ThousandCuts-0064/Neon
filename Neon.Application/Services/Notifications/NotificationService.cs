using System.Collections.Concurrent;
using Neon.Domain.Notifications.Bases;

namespace Neon.Application.Services.Notifications;

internal class NotificationService : INotificationService
{
    private readonly ConcurrentDictionary<Type, Notifier> _notifiers = [];

    public void Notify(Notification dbNotification)
    {
        if (_notifiers.TryGetValue(dbNotification.GetType(), out var handler))
            handler.Notify(dbNotification);
    }

    public void Listen<T>(Action<T> handler) where T : Notification
    {
        if (_notifiers.TryGetValue(typeof(T), out var notifier))
        {
            notifier.Listen(handler);

            return;
        }

        notifier = new Notifier<T>();
        notifier.Listen(handler);

        _notifiers.TryAdd(typeof(T), notifier);
    }

    public void Unlisten<T>(Action<T> handler) where T : Notification
    {
        if (_notifiers.TryGetValue(typeof(T), out var notifier))
            notifier.Unlisten(handler);
    }

    private abstract class Notifier
    {
        public abstract void Notify(Notification dbNotification);
        public abstract void Listen<T>(Action<T> handler) where T : Notification;
        public abstract void Unlisten<T>(Action<T> handler) where T : Notification;
    }

    private class Notifier<T> : Notifier where T : Notification
    {
        private Action<T>? _onNotify;

        public override void Notify(Notification dbNotification) => _onNotify?.Invoke((T)dbNotification);
        public override void Listen<T1>(Action<T1> handler) => _onNotify += (Action<T>)handler;
        public override void Unlisten<T1>(Action<T1> handler) => _onNotify -= (Action<T>)handler;
    }
}