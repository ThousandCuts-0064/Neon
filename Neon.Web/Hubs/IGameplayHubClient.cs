using Neon.Domain.DbNotifications;
using Neon.Domain.Enums;

namespace Neon.Web.Hubs;

public interface IGameplayHubClient
{
    Task AlreadyActive();
    Task ActiveConnectionToggle(ActiveConnectionToggle activeConnectionToggle);

    Task SendMessage(
        UserRole userRole, string usernamePrefix, string username, string usernameSuffix,
        string message, bool isImportant);

    Task ExecutedCommand(string usernamePrefix, string username, string usernameSuffix, string message);
    Task InvalidCommand(string usernamePrefix, string username, string usernameSuffix, string message);
}