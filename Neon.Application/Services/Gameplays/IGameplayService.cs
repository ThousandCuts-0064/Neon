using Neon.Application.Models;

namespace Neon.Application.Services.Gameplays;

public interface IGameplayService
{
    public Task<IReadOnlyCollection<TOpponentModel>> FindOpponentsAsync<TOpponentModel>(int userId)
        where TOpponentModel : IOpponentModel<TOpponentModel>;

    public Task GiveItem(int userId, Guid itemKey);
}