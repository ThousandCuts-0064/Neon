using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Models;
using Neon.Application.Projections;

namespace Neon.Application.Services.Lobbies;

internal class LobbyService : ILobbyService
{
    private readonly INeonDbContext _dbContext;

    public LobbyService(INeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TUserModel>> FindActiveUsersAsync<TUserModel>(int userId)
        where TUserModel : IUserModel<TUserModel>
    {
        return await _dbContext.Users
            .Where(x => x.Id != userId && x.ConnectionId != null)
            .Select(UserSecureProjection.FromEntity)
            .Select(TUserModel.FromProjection)
            .ToListAsync();
    }

    public async Task GiveItem(int userId, Guid itemKey)
    {
        // TODO
        Debugger.Break();
    }
}