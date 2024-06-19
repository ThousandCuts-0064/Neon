using Microsoft.AspNetCore.SignalR;

namespace Neon.Web.Hubs;

public class GameplayHub : Hub<IGameplayHubClient>
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
