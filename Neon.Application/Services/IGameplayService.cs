using Neon.Domain.Entities;

namespace Neon.Application.Services;

public interface IGameplayService
{
    public IQueryable<User> ActiveUsers { get; }

    public Task ClearUserConnectionsAsync();
    public Task<bool> TrySetActiveAsync(int userId, string connectionId);
    public Task<bool> TrySetInactiveAsync(int userId, string connectionId);
}
