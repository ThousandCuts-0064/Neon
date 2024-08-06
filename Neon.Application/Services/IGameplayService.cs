using Neon.Domain.Entities;

namespace Neon.Application.Services;

public interface IGameplayService
{
    public IQueryable<User> ActiveUsers { get; }

    public User GetUserByUsername(string username);

    public Task ClearUserConnectionsAsync();
    public Task<bool> TrySetUserActiveAsync(int userId, string connectionId);
    public Task<bool> TrySetUserInactiveAsync(int userId, string connectionId);

    public Task UserGetItem(int userId, Guid itemKey);
}
