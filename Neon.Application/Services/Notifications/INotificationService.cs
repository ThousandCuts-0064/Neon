namespace Neon.Application.Services.Notifications;

public interface INotificationService
{
    public void Notify(Domain.Notifications.Bases.Notification dbNotification);
    public void Listen<T>(Action<T> handler) where T : Domain.Notifications.Bases.Notification;
    public void Unlisten<T>(Action<T> handler) where T : Domain.Notifications.Bases.Notification;
}