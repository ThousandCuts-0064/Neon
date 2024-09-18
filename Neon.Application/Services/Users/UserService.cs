using Microsoft.EntityFrameworkCore;
using Neon.Application.Interfaces;
using Neon.Application.Projections;
using Neon.Application.Services.Bases;
using Neon.Domain.Entities.UserRequests.Bases;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Users;

internal class UserService : DbContextService, IUserService
{
    public UserService(INeonDbContext dbContext) : base(dbContext) { }

    public async Task<TModel> FindAsync<TModel>(int id)
        where TModel : IUserModel<TModel>
    {
        return await DbContext.Users
            .Where(x => x.Id == id)
            .Select(UserProjection.FromEntity)
            .Select(TModel.FromProjection)
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

    public async Task<IReadOnlyCollection<TModel>> FindFriendsAsync<TModel>(int id)
        where TModel : IUserModel<TModel>
    {
        return await DbContext.Friendships
            .Where(x => x.User1Id == id || x.User2Id == id)
            .Select(x => x.User1Id == id ? x.User2Id : x.User1Id)
            .Join(DbContext.Users, x => x, x => x.Id, (_, y) => y)
            .Select(UserProjection.FromEntity)
            .Select(TModel.FromProjection)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TModel>> FindIncomingUserRequests<TModel>(int id)
        where TModel : IIncomingUserRequestModel<TModel>
    {
        return await DbContext.DuelRequests
            .Select(DuelRequestProjection.BaseFromEntity)
            .Where(x => x.ResponderId == id)
            .Concat(DbContext.TradeRequests
                .Select(TradeRequestProjection.BaseFromEntity)
                .Where(x => x.ResponderId == id))
            .Concat(DbContext.FriendRequests
                .Select(FriendRequestProjection.BaseFromEntity)
                .Where(x => x.ResponderId == id))
            .OrderByDescending(x => x.CreatedAt)
            .Join(
                DbContext.Users,
                x => x.RequesterId,
                x => x.Id,
                UserRequestPolymorphicProjectionJoinUser.FromComponents)
            .Select(IncomingUserRequestProjection.FromJoin)
            .Select(TModel.FromProjection)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TModel>> FindOutgoingUserRequests<TModel>(int id)
        where TModel : IOutgoingUserRequestModel<TModel>
    {
        return await DbContext.DuelRequests
            .Select(DuelRequestProjection.BaseFromEntity)
            .Where(x => x.RequesterId == id)
            .Concat(DbContext.TradeRequests
                .Select(TradeRequestProjection.BaseFromEntity)
                .Where(x => x.RequesterId == id))
            .Concat(DbContext.FriendRequests
                .Select(FriendRequestProjection.BaseFromEntity)
                .Where(x => x.RequesterId == id))
            .OrderByDescending(x => x.CreatedAt)
            .Join(
                DbContext.Users,
                x => x.ResponderId,
                x => x.Id,
                UserRequestPolymorphicProjectionJoinUser.FromComponents)
            .Select(OutgoingUserRequestProjection.FromJoin)
            .Select(TModel.FromProjection)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TModel>> FindOtherActiveUsersAsync<TModel>(int id)
        where TModel : IUserModel<TModel>
    {
        return await DbContext.Users
            .Where(x => x.Id != id && x.ConnectionId != null)
            .Select(UserProjection.FromEntity)
            .Select(TModel.FromProjection)
            .ToListAsync();
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