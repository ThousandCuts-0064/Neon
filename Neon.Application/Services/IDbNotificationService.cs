using Neon.Domain.DbNotifications.Bases;

namespace Neon.Application.Services;

public interface IDbNotificationService
{
    public void Notify(DbNotification dbNotification);
    public void Listen<T>(Action<T> handler) where T : DbNotification;
    public void Unlisten<T>(Action<T> handler) where T : DbNotification;
}