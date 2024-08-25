using Neon.Web.Args.Client;
using Neon.Web.Args.Shared;

namespace Neon.Web.Hubs;

public interface ILobbyClient
{
    public Task ConnectedFromAnotherSource();
    public Task ActiveConnectionToggle(ActiveConnectionToggleArgs args);
    public Task SendMessage(UserMessageArgs args);
    public Task ExecutedCommand(CommandMessageArgs args);
    public Task InvalidCommand(CommandMessageArgs args);

    public Task SendFriendRequest(SendFriendRequestArgs args);
    public Task AcceptFriendRequest(AcceptFriendRequestArgs args);
    public Task DeclineFriendRequest(DeclineFriendRequestArgs args);
    public Task CancelFriendRequest(CancelFriendRequestArgs args);

    public Task SendTradeRequest(SendTradeRequestArgs args);
    public Task AcceptTradeRequest(AcceptTradeRequestArgs args);
    public Task DeclineTradeRequest(DeclineTradeRequestArgs args);
    public Task CancelTradeRequest(CancelTradeRequestArgs args);

    public Task SendDuelRequest(SendDuelRequestArgs args);
    public Task AcceptDuelRequest(AcceptDuelRequestArgs args);
    public Task DeclineDuelRequest(DeclineDuelRequestArgs args);
    public Task CancelDuelRequest(CancelDuelRequestArgs args);
}