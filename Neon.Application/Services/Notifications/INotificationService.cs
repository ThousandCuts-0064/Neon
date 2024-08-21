using Neon.Domain.Notifications.Bases;

namespace Neon.Application.Services.Notifications;

public interface INotificationService
{
    public void Notify(Notification dbNotification);
    public void Listen<T>(Action<T> handler) where T : Notification;
    public void Unlisten<T>(Action<T> handler) where T : Notification;
}