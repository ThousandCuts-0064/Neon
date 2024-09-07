using Microsoft.EntityFrameworkCore;
using Neon.Application.Models;
using Neon.Application.Projections;
using Neon.Application.Services.Bases;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Users;

internal class UserService : DbContextService, IUserService
{
    public UserService(INeonDbContext dbContext) : base(dbContext) { }

    public async Task<TUserModel> FindAsync<TUserModel>(int id) where TUserModel : IUserModel<TUserModel>
    {
        return await DbContext.Users
            .Where(x => x.Id == id)
            .Select(UserSecureProjection.FromEntity)
            .Select(TUserModel.FromProjection)
            .FirstAsync();
    }

    public async Task<int> FindIdAsync(Guid key)
    {
        return await DbContext.Users
            .Where(x => x.Key == key)
            .Select(x => x.Id)
            .FirstAsync();
    }

    public async Task<string> FindUsername(int id)
    {
        return await DbContext.Users
            .Where(x => x.Id == id)
            .Select(x => x.Username)
            .FirstAsync();
    }

    public async Task<UserRole> FindRoleAsync(int id)
    {
        return await DbContext.Users
            .Where(x => x.Id == id)
            .Select(x => x.Role)
            .FirstAsync();
    }

    public async Task<string?> SetActiveAsync(int id, string connectionId)
    {
        var oldConnectionId = await DbContext.Users
            .Where(x => x.Id == id)
            .Select(x => x.ConnectionId)
            .FirstAsync();

        await DbContext.Users
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ConnectionId, connectionId));

        return oldConnectionId;
    }

    public async Task SetInactiveAsync(int id, string connectionId)
    {
        await DbContext.Users
            .Where(x => x.Id == id && x.ConnectionId == connectionId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));
    }

    public async Task SetAllInactiveAsync(DateTime lastActiveAt)
    {
        await DbContext.Users
            .Where(x => x.ConnectionId != null)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, lastActiveAt));
    }
}