using Neon.Domain.Users;

namespace Neon.Application.Services;

public interface IGameplayService
{
    public IQueryable<User> ActiveUsers { get; }

    public Task SetActiveAsync(int userId);
    public Task SetInactiveAsync(int userId);
}
