using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Extensions;
using Neon.Application.Services.Lobbies;
using Neon.Application.Services.UserInputs;
using Neon.Application.Services.UserInputs.Enums;
using Neon.Application.Services.UserRequests.Bases;
using Neon.Application.Services.UserRequests.Duel;
using Neon.Application.Services.UserRequests.Friend;
using Neon.Application.Services.UserRequests.Trade;
using Neon.Application.Services.Users;
using Neon.Domain.Enums;
using Neon.Web.Args.Client;
using Neon.Web.Args.Hub;
using Neon.Web.Resources;

namespace Neon.Web.Hubs;

public class LobbyHub : Hub<ILobbyClient>
{
    private static readonly ConcurrentDictionary<string, HubCallerContext> _activeContexts = [];
    private readonly ILobbyService _lobbyService;
    private readonly IUserService _userService;
    private readonly IUserInputService _userInputService;
    private readonly FriendRequestHandler _friendRequestHandler;
    private readonly TradeRequestHandler _tradeRequestHandler;
    private readonly DuelRequestHandler _duelRequestHandler;

    private int UserId => int.Parse(Context.UserIdentifier!);
    private ClaimsPrincipal User => Context.User!;

    public LobbyHub(
        ILobbyService lobbyService,
        IUserService userService,
        IUserInputService userInputService,
        IFriendRequestService friendRequestService,
        ITradeRequestService tradeRequestService,
        IDuelRequestService duelRequestService)
    {
        _lobbyService = lobbyService;
        _userService = userService;
        _userInputService = userInputService;
        _friendRequestHandler = new FriendRequestHandler(friendRequestService);
        _tradeRequestHandler = new TradeRequestHandler(tradeRequestService);
        _duelRequestHandler = new DuelRequestHandler(duelRequestService);
    }

    public override async Task OnConnectedAsync()
    {
        var oldConnectionId = await _userService.SetActiveAsync(UserId, Context.ConnectionId);

        if (oldConnectionId is not null)
        {
            await Clients.Client(oldConnectionId).ConnectedFromAnotherSource();

            if (_activeContexts.TryRemove(oldConnectionId, out var hubCallerContext))
                hubCallerContext.Abort();
        }

        _activeContexts.TryAdd(Context.ConnectionId, Context);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _userService.SetInactiveAsync(UserId, Context.ConnectionId);
    }

    public async Task HandleInput(string text)
    {
        var userInputType = _userInputService.Handle(UserId, text, out var message);

        switch (userInputType)
        {
            case UserInputType.PlainText:
            case UserInputType.ImportantText:
                var role = await _userService.FindRoleAsync(UserId);

                var (usernamePrefix, usernameSuffix) = role switch
                {
                    UserRole.Guest => ("(", ")"),
                    UserRole.Standard => ("<", ">"),
                    UserRole.Admin => ("{", "}"),
                    _ => throw new UnreachableException()
                };

                await Clients.All.SendMessage(new UserMessageArgs
                {
                    UserRole = role,
                    UsernamePrefix = usernamePrefix,
                    Username = Context.User!.GetUsername(),
                    UsernameSuffix = usernameSuffix,
                    Message = message,
                    IsImportant = userInputType == UserInputType.ImportantText
                });

                break;

            case UserInputType.ExecutedCommand:
                await Clients.All.ExecutedCommand(new CommandMessageArgs
                {
                    UsernamePrefix = "[",
                    Username = Resource.Client_Generic_SystemName,
                    UsernameSuffix = "]",
                    Message = message
                });

                break;

            case UserInputType.InvalidCommand:
                await Clients.All.InvalidCommand(new CommandMessageArgs
                {
                    UsernamePrefix = "[",
                    Username = Resource.Client_Generic_SystemName,
                    UsernameSuffix = "]",
                    Message = message
                });

                break;
        }
    }


    public Task SendFriendRequest(SendFriendRequestArgs args) => _friendRequestHandler.SendAsync(User, args);
    public Task AcceptFriendRequest(AcceptFriendRequestArgs args) => _friendRequestHandler.AcceptAsync(User, args);
    public Task DeclineFriendRequest(DeclineFriendRequestArgs args) => _friendRequestHandler.DeclineAsync(User, args);
    public Task CancelFriendRequest(CancelFriendRequestArgs args) => _friendRequestHandler.CancelAsync(User, args);

    public Task SendTradeRequest(SendTradeRequestArgs args) => _tradeRequestHandler.SendAsync(User, args);
    public Task AcceptTradeRequest(AcceptTradeRequestArgs args) => _tradeRequestHandler.AcceptAsync(User, args);
    public Task DeclineTradeRequest(DeclineTradeRequestArgs args) => _tradeRequestHandler.DeclineAsync(User, args);
    public Task CancelTradeRequest(CancelTradeRequestArgs args) => _tradeRequestHandler.CancelAsync(User, args);

    public Task SendDuelRequest(SendDuelRequestArgs args) => _duelRequestHandler.SendAsync(User, args);
    public Task AcceptDuelRequest(AcceptDuelRequestArgs args) => _duelRequestHandler.AcceptAsync(User, args);
    public Task DeclineDuelRequest(DeclineDuelRequestArgs args) => _duelRequestHandler.DeclineAsync(User, args);
    public Task CancelDuelRequest(CancelDuelRequestArgs args) => _duelRequestHandler.CancelAsync(User, args);


    private class FriendRequestHandler : UserRequestHandler<
        IFriendRequestService,
        SendFriendRequestArgs, AcceptFriendRequestArgs, DeclineFriendRequestArgs, CancelFriendRequestArgs>
    {
        public FriendRequestHandler(IFriendRequestService userRequestService) : base(userRequestService) { }
    }

    private class TradeRequestHandler : UserRequestHandler<
        ITradeRequestService,
        SendTradeRequestArgs, AcceptTradeRequestArgs, DeclineTradeRequestArgs, CancelTradeRequestArgs>
    {
        public TradeRequestHandler(ITradeRequestService userRequestService) : base(userRequestService) { }
    }

    private class DuelRequestHandler : UserRequestHandler<
        IDuelRequestService,
        SendDuelRequestArgs, AcceptDuelRequestArgs, DeclineDuelRequestArgs, CancelDuelRequestArgs>
    {
        public DuelRequestHandler(IDuelRequestService userRequestService) : base(userRequestService) { }
    }

    private abstract class UserRequestHandler<
        TUserRequestService,
        TSendUserRequestArgs, TAcceptUserRequestArgs, TDeclineUserRequestArgs, TCancelUserRequestArgs>
        where TUserRequestService : IUserRequestService
        where TSendUserRequestArgs : SendUserRequestArgs
        where TAcceptUserRequestArgs : AcceptUserRequestArgs
        where TDeclineUserRequestArgs : DeclineUserRequestArgs
        where TCancelUserRequestArgs : CancelUserRequestArgs
    {
        private readonly TUserRequestService _userRequestService;

        protected UserRequestHandler(TUserRequestService userRequestService)
        {
            _userRequestService = userRequestService;
        }

        public async Task SendAsync(ClaimsPrincipal user, TSendUserRequestArgs args)
        {
            await _userRequestService.SendAsync(
                user.GetId(),
                user.GetKey(),
                user.GetUsername(),
                args.ResponderKey);
        }

        public async Task AcceptAsync(ClaimsPrincipal user, TAcceptUserRequestArgs args)
        {
            await _userRequestService.AcceptAsync(
                args.RequesterKey,
                user.GetId(),
                user.GetKey(),
                user.GetUsername());
        }

        public async Task DeclineAsync(ClaimsPrincipal user, TDeclineUserRequestArgs args)
        {
            await _userRequestService.DeclineAsync(
                args.RequesterKey,
                user.GetId(),
                user.GetKey(),
                user.GetUsername());
        }

        public async Task CancelAsync(ClaimsPrincipal user, TCancelUserRequestArgs args)
        {
            await _userRequestService.CancelAsync(
                user.GetId(),
                user.GetKey(),
                user.GetUsername(),
                args.ResponderKey);
        }
    }
}