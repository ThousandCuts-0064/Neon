using Neon.Application.Models;

namespace Neon.Application.Services.Lobbies;

public interface ILobbyService
{
    public Task<IReadOnlyCollection<TOpponentModel>> FindOpponentsAsync<TOpponentModel>(int userId)
        where TOpponentModel : IOpponentModel<TOpponentModel>;

    public Task GiveItem(int userId, Guid itemKey);
}