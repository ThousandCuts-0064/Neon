using Microsoft.EntityFrameworkCore;
using Neon.Application.Models;
using Neon.Application.Projections;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Users;

internal class UserService : IUserService
{
    private readonly INeonDbContext _dbContext;

    public UserService(INeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TUserModel> FindAsync<TUserModel>(int id) where TUserModel : IUserModel<TUserModel>
    {
        return await _dbContext.Users
            .Where(x => x.Id == id)
            .Select(UserSecureProjection.FromEntity)
            .Select(TUserModel.FromProjection)
            .FirstAsync();
    }

    public async Task<UserRole> FindRoleAsync(int id)
    {
        var role = await _dbContext.Users
            .Where(x => x.Id == id)
            .Join(_dbContext.UserRoles,
                x => x.Id,
                x => x.UserId,
                (x, y) => y.RoleId)
            .Join(_dbContext.Roles,
                x => x,
                x => x.Id,
                (x, y) => y.Name)
            .FirstAsync();

        return Enum.Parse<UserRole>(role!);
    }

    public async Task<string?> SetActiveAsync(int id, string connectionId)
    {
        var oldConnectionId = await _dbContext.Users
            .Where(x => x.Id == id)
            .Select(x => x.ActiveConnectionId)
            .FirstAsync();

        await _dbContext.Users
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, connectionId));

        return oldConnectionId;
    }

    public async Task SetInactiveAsync(int id, string connectionId)
    {
        await _dbContext.Users
            .Where(x => x.Id == id && x.ActiveConnectionId == connectionId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, connectionId)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));
    }

    public async Task SetAllInactiveAsync()
    {
        await _dbContext.Users
            .Where(x => x.ActiveConnectionId != null)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));
    }
}