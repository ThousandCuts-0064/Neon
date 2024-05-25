using System.Diagnostics.CodeAnalysis;
using Neon.Domain;

namespace Neon.Application.Services.Users;

internal class UserService : IUserService
{
    private readonly INeonDomain _domain;

    public UserService(INeonDomain domain)
    {
        _domain = domain;
    }

    public bool CreateGuest(string username, out int id)
    {
        //if (_domain.UserRepository.ContainsUsername(username))
        //    return false;

        id = 1;

        //_domain.UserRepository.Add(new User
        //{
        //    Username = username,
        //    Secret = id,
        //    Role = UserRole.Guest,
        //    LastActiveAt = DateTime.Now
        //});

        //_domain.SaveChanges();

        return true;
    }

    public AuthenticateResult Authenticate(string username, string secret)
    {
        return AuthenticateResult.Success;
    }
}

public enum AuthenticateResult
{
    UsernameNotFound,
    SecretMismatch,
    Success
}