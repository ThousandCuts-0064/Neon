using Neon.Application.Models;

namespace Neon.Application.Services.Lobbies;

public interface ILobbyService
{
    public Task<IReadOnlyCollection<TUserModel>> FindActiveUsersAsync<TUserModel>(int userId)
        where TUserModel : IUserModel<TUserModel>;

    public Task GiveItem(int userId, Guid itemKey);
}