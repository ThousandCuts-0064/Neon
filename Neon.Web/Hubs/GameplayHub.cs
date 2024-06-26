﻿using Microsoft.AspNetCore.SignalR;
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
        await _gameplayService.SetActiveAsync(UserId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _gameplayService.SetInactiveAsync(UserId);
    }
}