using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Extensions;
using Neon.Application.Factories.Principals;
using Neon.Application.Interfaces;
using Neon.Application.Services.Bases;
using Neon.Application.Services.Passwords;
using Neon.Application.Validators.Users;
using Neon.Domain.Entities;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Authentications;

internal class AuthenticationService : DbContextService, IAuthenticationService
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordService _passwordService;
    private readonly IPrincipalFactory _principalFactory;
    private readonly IUserValidator _userValidator;

    public AuthenticationService(
        INeonDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        IPasswordService passwordService,
        IPrincipalFactory principalFactory,
        IUserValidator userValidator)
        : base(dbContext)
    {
        _httpContext = httpContextAccessor.HttpContext!;
        _passwordService = passwordService;
        _principalFactory = principalFactory;
        _userValidator = userValidator;
    }

    public async Task<RegisterResult> GuestAsync(string username, bool rememberMe)
    {
        if (!_userValidator.IsUsernameValid(username))
            return RegisterResult.UsernameInvalidCharacters;

        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var isUsernameTaken = await DbContext.Users.AnyAsync(x => x.Username == username);

        if (isUsernameTaken)
            return RegisterResult.UsernameTaken;

        var user = NewUser(username, UserRole.Guest);

        DbContext.Users.Add(user);

        await DbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        await SignInAsync(user, rememberMe);

        return RegisterResult.Success;
    }

    public async Task<LoginResult> LoginAsync(string username, string password, bool rememberMe)
    {
        if (!_userValidator.IsUsernameValid(username) || !_userValidator.IsPasswordValid(password))
            return LoginResult.UsernameNotFound;

        var user = await DbContext.Users
            .Where(x => x.Username == username)
            .Select(x => new
            {
                x.Id,
                x.Key,
                x.Username,
                x.PasswordHash,
                x.Role,
                x.SecurityKey,
                x.IdentityKey
            })
            .FirstOrDefaultAsync();

        if (user is null)
            return LoginResult.UsernameNotFound;

        if (user.PasswordHash is null)
            return LoginResult.CannotLogInGuest;

        var isPasswordCorrect = await _passwordService.CompareAsync(user.PasswordHash, password);

        if (!isPasswordCorrect)
            return LoginResult.WrongPassword;

        await SignInAsync(user.Id, user.Key, user.Username, user.Role, user.SecurityKey, user.IdentityKey, rememberMe);

        return LoginResult.Success;
    }

    public async Task<RegisterResult> RegisterAsync(string username, string password, bool rememberMe)
    {
        if (!_userValidator.IsUsernameValid(username))
            return RegisterResult.UsernameInvalidCharacters;

        if (!_userValidator.IsPasswordValid(password))
            return RegisterResult.WeakPassword;

        await using var transaction = await DbContext.Database.BeginTransactionAsync();

        var isUsernameTaken = await DbContext.Users.AnyAsync(x => x.Username == username);

        if (isUsernameTaken)
            return RegisterResult.UsernameTaken;

        var passwordHash = await _passwordService.HashAsync(password);
        var user = NewUser(username, UserRole.Standard, passwordHash);

        DbContext.Users.Add(user);

        await DbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        await SignInAsync(user, rememberMe);

        return RegisterResult.Success;
    }

    public async Task LogoutAsync()
    {
        var userId = _httpContext.User.GetId();

        await DbContext.Users
            .Where(x => x.Id == userId && x.Role == UserRole.Guest)
            .ExecuteDeleteAsync();

        await _httpContext.SignOutAsync();
    }

    private static User NewUser(string username, UserRole role, string? passwordHash = null) => new()
    {
        Key = Guid.NewGuid(),
        Username = username,
        PasswordHash = passwordHash,
        Role = role,
        RegisteredAt = DateTime.UtcNow,
        LastActiveAt = DateTime.UtcNow,
        SecurityKey = Guid.NewGuid(),
        IdentityKey = Guid.NewGuid()
    };

    private Task SignInAsync(User user, bool rememberMe)
    {
        return SignInAsync(
            user.Id,
            user.Key,
            user.Username,
            user.Role,
            user.SecurityKey,
            user.IdentityKey,
            rememberMe);
    }

    private Task SignInAsync(
        int userId, Guid key, string username, UserRole role, Guid securityKey, Guid identityKey,
        bool rememberMe)
    {
        var claimsPrincipal = _principalFactory.Create(userId, key, username, role, securityKey, identityKey);
        var authenticationProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe
        };

        return _httpContext.SignInAsync(claimsPrincipal, authenticationProperties);
    }
}