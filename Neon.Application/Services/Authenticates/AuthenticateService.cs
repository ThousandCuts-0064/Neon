using Microsoft.AspNetCore.Identity;
using Neon.Domain.Entities;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Authenticates;

internal class AuthenticateService : IAuthenticateService
{
    private readonly INeonDbContext _dbContext;
    private readonly SignInManager<User> _signInManager;
    private UserManager<User> UserManager => _signInManager.UserManager;

    public AuthenticateService(INeonDbContext dbContext, SignInManager<User> signInManager)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
    }

    public async Task<RegisterResult> GuestAsync(string username, bool rememberMe)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = NewUser(username);

        var result = await UserManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            if (result.Errors.Count() > 1)
                return RegisterResult.Error;

            return result.Errors.First().Code switch
            {
                nameof(IdentityErrorDescriber.InvalidUserName) => RegisterResult.UsernameInvalidCharacters,
                nameof(IdentityErrorDescriber.DuplicateUserName) => RegisterResult.UsernameTaken,
                _ => RegisterResult.Error
            };
        }

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

        var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, true);

        if (result.IsNotAllowed)
            return LoginResult.WrongPassword;

        if (!result.Succeeded)
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

    public async Task LogoutAsync()
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        await _signInManager.SignOutAsync();

        var user = await UserManager.FindByNameAsync(_signInManager.Context.User.Identity!.Name!);

        if (await UserManager.IsInRoleAsync(user!, nameof(UserRole.Guest)))
            await UserManager.DeleteAsync(user!);

        await transaction.CommitAsync();
    }

    private static User NewUser(string username) => new()
    {
        UserName = username,
        RegisteredAt = DateTime.UtcNow,
    };
}