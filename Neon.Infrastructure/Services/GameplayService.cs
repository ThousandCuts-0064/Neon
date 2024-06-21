using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.Users;

namespace Neon.Infrastructure.Services;

internal class GameplayService : IGameplayService
{
    private readonly NeonDbContext _dbContext;
    public IQueryable<User> ActiveUsers => _dbContext.Users.Where(x => x.IsActive);

    public GameplayService(NeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SetActiveAsync(int userId)
    {
        await _dbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.IsActive, true));
    }

    public async Task SetInactiveAsync(int userId)
    {
        await _dbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.IsActive, false)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));
    }
}