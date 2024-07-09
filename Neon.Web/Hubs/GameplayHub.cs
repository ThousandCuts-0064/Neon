using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Services;

namespace Neon.Web.Hubs;

public class GameplayHub : Hub<IGameplayHubClient>
{
    private readonly IGameplayService _gameplayService;
    private int UserId => int.Parse(Context.UserIdentifier!);

    public GameplayHub(IGameplayService gameplayService)
    {
        _gameplayService = gameplayService;
    }

    public override async Task OnConnectedAsync()
    {
        if (await _gameplayService.TrySetActiveAsync(UserId, Context.ConnectionId))
            return;

        await Clients.Caller.AlreadyActive();
        await Context.Features
            .GetRequiredFeature<IHttpContextFeature>()
            .HttpContext!
            .SignOutAsync();
        Context.Abort();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _gameplayService.TrySetInactiveAsync(UserId, Context.ConnectionId);
    }
}