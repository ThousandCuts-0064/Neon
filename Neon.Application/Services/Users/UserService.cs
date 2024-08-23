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

    public async Task<int> FindIdAsync(string username)
    {
        return await DbContext.Users
            .Where(x => x.UserName == username)
            .Select(x => x.Id)
            .FirstAsync();
    }

    public async Task<UserRole> FindRoleAsync(int id)
    {
        var role = await DbContext.Users
            .Where(x => x.Id == id)
            .Join(DbContext.UserRoles,
                x => x.Id,
                x => x.UserId,
                (x, y) => y.RoleId)
            .Join(DbContext.Roles,
                x => x,
                x => x.Id,
                (x, y) => y.Name)
            .FirstAsync();

        return Enum.Parse<UserRole>(role!);
    }

    public async Task<string?> SetActiveAsync(int id, string connectionId)
    {
        var oldConnectionId = await DbContext.Users
            .Where(x => x.Id == id)
            .Select(x => x.ActiveConnectionId)
            .FirstAsync();

        await DbContext.Users
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, connectionId));

        return oldConnectionId;
    }

    public async Task SetInactiveAsync(int id, string connectionId)
    {
        await DbContext.Users
            .Where(x => x.Id == id && x.ActiveConnectionId == connectionId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, DateTime.UtcNow));
    }

    public async Task SetAllInactiveAsync(DateTime lastActiveAt)
    {
        await DbContext.Users
            .Where(x => x.ActiveConnectionId != null)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.ActiveConnectionId, (string?)null)
                .SetProperty(y => y.LastActiveAt, lastActiveAt));
    }
}