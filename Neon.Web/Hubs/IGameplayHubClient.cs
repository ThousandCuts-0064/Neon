using Neon.Domain.DbNotifications;

namespace Neon.Web.Hubs;

public interface IGameplayHubClient
{
    Task AlreadyActive();
    Task ActiveConnectionToggle(ActiveConnectionToggle activeConnectionToggle);
    Task SendMessage(string username, string message);
    Task ExecutedCommand(string message);
    Task InvalidCommand(string message);
}
