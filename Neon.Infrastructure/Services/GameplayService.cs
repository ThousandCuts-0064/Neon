using Microsoft.AspNetCore.Identity;
using Neon.Application.Services;
using Neon.Domain.Users;

namespace Neon.Infrastructure.Services;

internal class GameplayService : IGameplayService
{
    private readonly UserManager<User> _userManager;
    public IQueryable<User> OnlineUsers => _userManager.Users.Where(x => x.IsActive);

    public GameplayService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
}
