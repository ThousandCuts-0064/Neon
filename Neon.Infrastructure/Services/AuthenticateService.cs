using Microsoft.AspNetCore.Identity;
using Neon.Application.Services;
using Neon.Data;
using Neon.Domain.Users;

namespace Neon.Infrastructure.Services;

internal class AuthenticateService : IAuthenticateService
{
    private readonly NeonDbContext _dbContext;
    private readonly SignInManager<User> _signInManager;
    private UserManager<User> UserManager => _signInManager.UserManager;

    public AuthenticateService(NeonDbContext dbContext, SignInManager<User> signInManager)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
    }

    public async Task<RegisterResult> GuestAsync(string username, bool rememberMe)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = NewUser(username);

        if (await UserManager.FindByNameAsync(username) is not null)
            return RegisterResult.UsernameTaken;

        var result = await UserManager.CreateAsync(user);

        if (!result.Succeeded)
            return RegisterResult.UsernameTaken;

        result = await UserManager.AddToRoleAsync(user, nameof(UserRole.Guest));

        if (!result.Succeeded)
            return RegisterResult.Error;

        await _signInManager.SignInAsync(user, rememberMe);

        await transaction.CommitAsync();

        return RegisterResult.Success;
    }

    public async Task<LoginResult> LoginAsync(string username, string password, bool rememberMe)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = await UserManager.FindByNameAsync(username);

        if (user is null)
            return LoginResult.UsernameNotFound;

        user.LastActiveAt = DateTime.UtcNow;
        user.IsActive = true;

        var update = await UserManager.UpdateAsync(user);

        if (!update.Succeeded)
            return LoginResult.Error;

        var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, true);

        if (result.IsNotAllowed)
            return LoginResult.WrongPassword;

        if (!update.Succeeded)
            return LoginResult.Error;

        await transaction.CommitAsync();

        return LoginResult.Success;
    }

    public async Task<RegisterResult> RegisterAsync(string username, string password, bool rememberMe)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = NewUser(username);

        if (await UserManager.FindByNameAsync(username) is not null)
            return RegisterResult.UsernameTaken;

        var result = await UserManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return RegisterResult.Error;

        result = await UserManager.AddToRoleAsync(user, nameof(UserRole.Standard));

        if (!result.Succeeded)
            return RegisterResult.Error;

        await _signInManager.SignInAsync(user, rememberMe);

        await transaction.CommitAsync();

        return RegisterResult.Success;
    }

    private static User NewUser(string username) => new()
    {
        UserName = username,
        LastActiveAt = DateTime.UtcNow,
        RegisteredAt = DateTime.UtcNow,
        IsActive = true
    };
}