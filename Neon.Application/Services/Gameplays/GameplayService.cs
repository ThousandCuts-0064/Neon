using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Models;
using Neon.Application.Projections;

namespace Neon.Application.Services.Gameplays;

internal class GameplayService : IGameplayService
{
    private readonly INeonDbContext _dbContext;

    public GameplayService(INeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TOpponentModel>> FindOpponentsAsync<TOpponentModel>(int userId)
        where TOpponentModel : IOpponentModel<TOpponentModel>
    {
        return await _dbContext.Users
            .Where(x => x.Id != userId && x.ActiveConnectionId != null)
            .Select(UserSecureProjection.FromEntity)
            .Select(TOpponentModel.FromProjection)
            .ToListAsync();
    }

    public async Task GiveItem(int userId, Guid itemKey)
    {
        // TODO
        Debugger.Break();
    }
}