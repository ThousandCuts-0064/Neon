﻿using Microsoft.EntityFrameworkCore;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.Entities;
using Neon.Infrastructure.Services.Abstracts;

namespace Neon.Infrastructure.Services;

internal class GameplayService : DbContextService, IGameplayService
{
    private readonly ISystemValueService _systemValueService;

    public IQueryable<User> ActiveUsers => DbContext.Users.Where(x => x.ActiveConnectionId != null);

    public GameplayService(NeonDbContext dbContext, ISystemValueService systemValueService) : base(dbContext)
    {
        _systemValueService = systemValueService;
    }

    public async Task ClearUserConnectionsAsync()
    {
        if (!await DbContext.Users.AnyAsync())
            return;

        var lastActiveAt = await _systemValueService.GetLastActiveAtAsync();

        await ActiveUsers
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, lastActiveAt));
    }

    public async Task<bool> TrySetActiveAsync(int userId, string connectionId)
    {
        var updatedCount = await DbContext.Users
            .Where(x => x.Id == userId && x.ActiveConnectionId == null)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, connectionId));

        return updatedCount != 0;
    }

    public async Task<bool> TrySetInactiveAsync(int userId, string connectionId)
    {
        var updatedCount = await DbContext.Users
            .Where(x => x.Id == userId && x.ActiveConnectionId == connectionId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));

        return updatedCount != 0;
    }
}