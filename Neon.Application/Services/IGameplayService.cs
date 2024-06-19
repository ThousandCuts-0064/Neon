using Neon.Domain.Users;

namespace Neon.Application.Services;

public interface IGameplayService
{
    public IQueryable<User> OnlineUsers { get; }
}
