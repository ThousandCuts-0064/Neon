using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Services.UserInputs;
using Neon.Application.Services.UserInputs.Enums;
using Neon.Application.Services.Users;
using Neon.Domain.Enums;
using Neon.Web.Args;
using Neon.Web.Utils.Extensions;

namespace Neon.Web.Hubs;

public class GameplayHub : Hub<IGameplayHubClient>
{
    private static readonly ConcurrentDictionary<string, HubCallerContext> _activeContexts = [];
    private readonly IUserInputService _userInputService;
    private readonly IUserService _userService;
    private int UserId => int.Parse(Context.UserIdentifier!);

    public GameplayHub(IUserInputService userInputService, IUserService userService)
    {
        _userInputService = userInputService;
        _userService = userService;
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
}