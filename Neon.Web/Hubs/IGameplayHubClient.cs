using Neon.Domain.DbNotifications;

namespace Neon.Web.Hubs;

public interface IGameplayHubClient
{
    Task AlreadyActive();
    Task ActiveConnectionToggle(ActiveConnectionToggle activeConnectionToggle);
}
