using Microsoft.AspNetCore.SignalR;
using Neon.Application;
using Neon.Application.Interfaces;
using Neon.Application.Services.Notifications;
using Neon.Domain.Notifications;
using Neon.Web.Args.Client;
using Neon.Web.Hubs;

namespace Neon.Web;

public class Initializer : IInitializer
{
    private readonly INotificationService _notificationService;
    private readonly IHubContext<LobbyHub, ILobbyClient> _lobbyHubContext;

    public Initializer(INotificationService notificationService, IHubContext<LobbyHub, ILobbyClient> lobbyHubContext)
    {
        _notificationService = notificationService;
        _lobbyHubContext = lobbyHubContext;
    }

    public ValueTask RunAsync()
    {
        ForwardUserConnectionToggled();

        ForwardUserRequestSent<FriendRequestSent, FriendRequestSentArgs>(y => y.FriendRequestSent);
        ForwardUserRequestAccepted<FriendRequestAccepted, FriendRequestAcceptedArgs>(y => y.FriendRequestAccepted);
        ForwardUserRequestDeclined<FriendRequestDeclined, FriendRequestDeclinedArgs>(y => y.FriendRequestDeclined);
        ForwardUserRequestCanceled<FriendRequestCanceled, FriendRequestCanceledArgs>(y => y.FriendRequestCanceled);

        ForwardUserRequestSent<TradeRequestSent, TradeRequestSentArgs>(y => y.TradeRequestSent);
        ForwardUserRequestAccepted<TradeRequestAccepted, TradeRequestAcceptedArgs>(y => y.TradeRequestAccepted);
        ForwardUserRequestDeclined<TradeRequestDeclined, TradeRequestDeclinedArgs>(y => y.TradeRequestDeclined);
        ForwardUserRequestCanceled<TradeRequestCanceled, TradeRequestCanceledArgs>(y => y.TradeRequestCanceled);

        ForwardUserRequestSent<DuelRequestSent, DuelRequestSentArgs>(y => y.DuelRequestSent);
        ForwardUserRequestAccepted<DuelRequestAccepted, DuelRequestAcceptedArgs>(y => y.DuelRequestAccepted);
        ForwardUserRequestDeclined<DuelRequestDeclined, DuelRequestDeclinedArgs>(y => y.DuelRequestDeclined);
        ForwardUserRequestCanceled<DuelRequestCanceled, DuelRequestCanceledArgs>(y => y.DuelRequestCanceled);

        return ValueTask.CompletedTask;
    }

    private void ForwardUserConnectionToggled()
    {
        _notificationService.Listen<UserConnectionToggled>(y =>
        {
            _lobbyHubContext.Clients.All.UserConnectionToggled(new UserConnectionToggledArgs
            {
                Key = y.Key,
                Username = y.Username,
                IsActive = y.IsActive
            });
        });
    }

    private void ForwardUserRequestSent<TUserRequest, TServerUserRequestArgs>(
        Func<ILobbyClient, Func<TServerUserRequestArgs, Task>> methodSelector)
        where TUserRequest : UserRequestSent
        where TServerUserRequestArgs : UserRequestSentArgs, new()
    {
        _notificationService.Listen<TUserRequest>(y =>
        {
            var client = _lobbyHubContext.Clients.User(y.ResponderId.ToString());

            methodSelector(client)(new TServerUserRequestArgs
            {
                RequesterKey = y.RequesterKey,
                RequesterUsername = y.RequesterUsername
            });
        });
    }

    private void ForwardUserRequestAccepted<TUserRequest, TServerUserRequestArgs>(
        Func<ILobbyClient, Func<TServerUserRequestArgs, Task>> methodSelector)
        where TUserRequest : UserRequestAccepted
        where TServerUserRequestArgs : UserRequestAcceptedArgs, new()
    {
        _notificationService.Listen<TUserRequest>(y =>
        {
            var client = _lobbyHubContext.Clients.User(y.RequesterId.ToString());

            methodSelector(client)(new TServerUserRequestArgs
            {
                ResponderKey = y.ResponderKey,
                ResponderUsername = y.ResponderUsername
            });
        });
    }

    private void ForwardUserRequestDeclined<TUserRequest, TServerUserRequestArgs>(
        Func<ILobbyClient, Func<TServerUserRequestArgs, Task>> methodSelector)
        where TUserRequest : UserRequestDeclined
        where TServerUserRequestArgs : UserRequestDeclinedArgs, new()
    {
        _notificationService.Listen<TUserRequest>(y =>
        {
            var client = _lobbyHubContext.Clients.User(y.RequesterId.ToString());

            methodSelector(client)(new TServerUserRequestArgs
            {
                ResponderKey = y.ResponderKey,
                ResponderUsername = y.ResponderUsername
            });
        });
    }

    private void ForwardUserRequestCanceled<TUserRequest, TServerUserRequestArgs>(
        Func<ILobbyClient, Func<TServerUserRequestArgs, Task>> methodSelector)
        where TUserRequest : UserRequestCanceled
        where TServerUserRequestArgs : UserRequestCanceledArgs, new()
    {
        _notificationService.Listen<TUserRequest>(y =>
        {
            var client = _lobbyHubContext.Clients.User(y.ResponderId.ToString());

            methodSelector(client)(new TServerUserRequestArgs
            {
                RequesterKey = y.RequesterKey,
                RequesterUsername = y.RequesterUsername
            });
        });
    }
}