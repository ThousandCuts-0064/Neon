using Neon.Web.Args;

namespace Neon.Web.Hubs;

public interface IGameplayHubClient
{
    Task ConnectedFromAnotherSource();
    Task ActiveConnectionToggle(ActiveConnectionToggleArgs activeConnectionToggle);
    Task SendMessage(UserMessageArgs args);
    Task ExecutedCommand(CommandMessageArgs args);
    Task InvalidCommand(CommandMessageArgs args);
}