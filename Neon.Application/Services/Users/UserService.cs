using Microsoft.AspNetCore.Identity;
using Neon.Domain;
using Neon.Domain.Users;

namespace Neon.Application.Services.Users;

internal class UserService : IUserService
{
    private readonly INeonDomain _domain;
    private readonly UserManager<User> _userManager;

    public UserService(INeonDomain domain, UserManager<User> userManager)
    {
        _domain = domain;
        _userManager = userManager;
    }

    public async Task<IdentityResult> GuestAsync(string username)
    {
        var user = new User
        {
            UserName = username,
            Role = UserRole.Guest,
            LastActiveAt = DateTime.Now,
            RegisteredAt = DateTime.Now,
        };

        if (await _userManager.FindByNameAsync(username) is null)
            return await _userManager.CreateAsync(user);

        return IdentityResult.Success;
    }

    public Task<LoginResult> LoginAsync(string username, string password)
    {
        return LoginResult.Success;
    }

    public Task<RegisterResult> RegisterAsync(string username, string password)
    {
        return RegisterResult.Success;
    }
}