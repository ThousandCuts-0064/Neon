using System.Diagnostics.CodeAnalysis;
using Neon.Domain;
using Neon.Domain.Users;

namespace Neon.Application.Services.Users;

internal class UserService : IUserService
{
    private readonly INeonDomain _domain;

    public UserService(INeonDomain domain)
    {
        _domain = domain;
    }

    public bool CreateGuest(string username, [NotNullWhen(true)] out string? secret)
    {
        secret = null;

        //if (_domain.UserRepository.ContainsUsername(username))
        //    return false;

        secret = Guid.NewGuid().ToString();

        //_domain.UserRepository.Add(new User
        //{
        //    Username = username,
        //    Secret = secret,
        //    Role = UserRole.Guest,
        //    LastActiveAt = DateTime.Now
        //});

        //_domain.SaveChanges();

        return true;
    }

    public bool AuthenticateGuest(string username, string secret)
    {
        return false;
    }
}
