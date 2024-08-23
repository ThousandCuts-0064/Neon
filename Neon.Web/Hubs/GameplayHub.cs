using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Services.Gameplays;
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
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Hubs;

public class GameplayHub : Hub<IGameplayHubClient>
{
    private static readonly ConcurrentDictionary<string, HubCallerContext> _activeContexts = [];
    private readonly IGameplayService _gameplayService;
    private readonly IUserService _userService;
    private readonly IUserInputService _userInputService;
    private readonly FriendRequestHandler _friendRequestHandler;
    private readonly TradeRequestHandler _tradeRequestHandler;
    private readonly DuelRequestHandler _duelRequestHandler;

    private int UserId => int.Parse(Context.UserIdentifier!);

    public GameplayHub(
        IGameplayService gameplayService,
        IUserService userService,
        IUserInputService userInputService,
        IFriendRequestService friendRequestService,
        ITradeRequestService tradeRequestService,
        IDuelRequestService duelRequestService)
    {
        _gameplayService = gameplayService;
        _userService = userService;
        _userInputService = userInputService;
        _friendRequestHandler = new FriendRequestHandler(_userService, friendRequestService);
        _tradeRequestHandler = new TradeRequestHandler(_userService, tradeRequestService);
        _duelRequestHandler = new DuelRequestHandler(_userService, duelRequestService);
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
                    Username = "System",
                    UsernameSuffix = "]",
                    Message = message
                });

                break;

            case UserInputType.InvalidCommand:
                await Clients.All.InvalidCommand(new CommandMessageArgs
                {
                    UsernamePrefix = "[",
                    Username = "System",
                    UsernameSuffix = "]",
                    Message = message
                });

                break;
        }
    }


    public Task SendFriendRequest(SendFriendRequestArgs args) => _friendRequestHandler.Send(UserId, args);
    public Task AcceptFriendRequest(AcceptFriendRequestArgs args) => _friendRequestHandler.Accept(UserId, args);
    public Task DeclineFriendRequest(DeclineFriendRequestArgs args) => _friendRequestHandler.Decline(UserId, args);
    public Task CancelFriendRequest(CancelFriendRequestArgs args) => _friendRequestHandler.Cancel(UserId, args);

    public Task SendTradeRequest(SendTradeRequestArgs args) => _tradeRequestHandler.Send(UserId, args);
    public Task AcceptTradeRequest(AcceptTradeRequestArgs args) => _tradeRequestHandler.Accept(UserId, args);
    public Task DeclineTradeRequest(DeclineTradeRequestArgs args) => _tradeRequestHandler.Decline(UserId, args);
    public Task CancelTradeRequest(CancelTradeRequestArgs args) => _tradeRequestHandler.Cancel(UserId, args);

    public Task SendDuelRequest(SendDuelRequestArgs args) => _duelRequestHandler.Send(UserId, args);
    public Task AcceptDuelRequest(AcceptDuelRequestArgs args) => _duelRequestHandler.Accept(UserId, args);
    public Task DeclineDuelRequest(DeclineDuelRequestArgs args) => _duelRequestHandler.Decline(UserId, args);
    public Task CancelDuelRequest(CancelDuelRequestArgs args) => _duelRequestHandler.Cancel(UserId, args);


    private class FriendRequestHandler : UserRequestHandler<
        IFriendRequestService,
        SendFriendRequestArgs, AcceptFriendRequestArgs, DeclineFriendRequestArgs, CancelFriendRequestArgs>
    {
        public FriendRequestHandler(IUserService userService, IFriendRequestService userRequestService) : base(
            userService, userRequestService) { }
    }

    private class TradeRequestHandler : UserRequestHandler<
        ITradeRequestService,
        SendTradeRequestArgs, AcceptTradeRequestArgs, DeclineTradeRequestArgs, CancelTradeRequestArgs>
    {
        public TradeRequestHandler(IUserService userService, ITradeRequestService userRequestService) : base(
            userService, userRequestService) { }
    }

    private class DuelRequestHandler : UserRequestHandler<
        IDuelRequestService,
        SendDuelRequestArgs, AcceptDuelRequestArgs, DeclineDuelRequestArgs, CancelDuelRequestArgs>
    {
        public DuelRequestHandler(IUserService userService, IDuelRequestService userRequestService) : base(
            userService, userRequestService) { }
    }

    private abstract class UserRequestHandler<
        TUserRequestService,
        TSendUserRequestArgs, TAcceptUserRequestArgs, TDeclineUserRequestArgs, TCancelUserRequestArgs>
        where TUserRequestService : IUserRequestService
        where TSendUserRequestArgs : ISendUserRequestArgs
        where TAcceptUserRequestArgs : IAcceptUserRequestArgs
        where TDeclineUserRequestArgs : IDeclineUserRequestArgs
        where TCancelUserRequestArgs : ICancelUserRequestArgs
    {
        private readonly IUserService _userService;
        private readonly TUserRequestService _userRequestService;

        protected UserRequestHandler(IUserService userService, TUserRequestService userRequestService)
        {
            _userService = userService;
            _userRequestService = userRequestService;
        }

        public async Task Send(int senderUserId, TSendUserRequestArgs args)
        {
            var responderUserId = await _userService.FindIdAsync(args.ResponderUsername);

            await _userRequestService.SendAsync(senderUserId, responderUserId);
        }

        public async Task Accept(int senderUserId, TAcceptUserRequestArgs args)
        {
            var responderUserId = await _userService.FindIdAsync(args.ResponderUsername);

            await _userRequestService.AcceptAsync(senderUserId, responderUserId);
        }

        public async Task Decline(int senderUserId, TDeclineUserRequestArgs args)
        {
            var responderUserId = await _userService.FindIdAsync(args.ResponderUsername);

            await _userRequestService.DeclineAsync(senderUserId, responderUserId);
        }

        public async Task Cancel(int senderUserId, TCancelUserRequestArgs args)
        {
            var responderUserId = await _userService.FindIdAsync(args.ResponderUsername);

            await _userRequestService.CancelAsync(senderUserId, responderUserId);
        }
    }
}