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
            LastActiveAt = DateTime.UtcNow,
            RegisteredAt = DateTime.UtcNow
        };

        if (await _userManager.FindByNameAsync(username) is not null)
            return IdentityResult.Success;

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
            return result;

        return await _userManager.AddToRoleAsync(user, nameof(UserRole.Guest));

    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        return LoginResult.Success;
    }

    public async Task<RegisterResult> RegisterAsync(string username, string password)
    {
        return RegisterResult.Success;
    }
}