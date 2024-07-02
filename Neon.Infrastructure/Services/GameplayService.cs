using Microsoft.EntityFrameworkCore;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.Users;

namespace Neon.Infrastructure.Services;

internal class GameplayService : IGameplayService
{
    private readonly NeonDbContext _dbContext;
    public IQueryable<User> ActiveUsers => _dbContext.Users.Where(x => x.ActiveConnectionId != null);

    public GameplayService(NeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> TrySetActiveAsync(int userId, string connectionId)
    {
        var affected = await _dbContext.Users
            .Where(x => x.Id == userId && x.ActiveConnectionId == null)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, connectionId));

        return affected != 0;
    }

    public async Task<bool> TrySetInactiveAsync(int userId, string connectionId)
    {
        var affected = await _dbContext.Users
            .Where(x => x.Id == userId && x.ActiveConnectionId == connectionId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));

        return affected != 0;
    }
}