using Neon.Web.Args.Client;

namespace Neon.Web.Hubs;

public interface ILobbyClient
{
    public Task ConnectedFromAnotherSource();
    public Task UserConnectionToggled(UserConnectionToggledArgs args);
    public Task SendMessage(UserMessageArgs args);
    public Task ExecutedCommand(CommandMessageArgs args);
    public Task InvalidCommand(CommandMessageArgs args);

    public Task FriendRequestSent(FriendRequestSentArgs args);
    public Task FriendRequestAccepted(FriendRequestAcceptedArgs args);
    public Task FriendRequestDeclined(FriendRequestDeclinedArgs args);
    public Task FriendRequestCanceled(FriendRequestCanceledArgs args);

    public Task TradeRequestSent(TradeRequestSentArgs args);
    public Task TradeRequestAccepted(TradeRequestAcceptedArgs args);
    public Task TradeRequestDeclined(TradeRequestDeclinedArgs args);
    public Task TradeRequestCanceled(TradeRequestCanceledArgs args);

    public Task DuelRequestSent(DuelRequestSentArgs args);
    public Task DuelRequestAccepted(DuelRequestAcceptedArgs args);
    public Task DuelRequestDeclined(DuelRequestDeclinedArgs args);
    public Task DuelRequestCanceled(DuelRequestCanceledArgs args);
}