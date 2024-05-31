using Neon.Domain;

namespace Neon.Application.Services.Users;

internal class UserService : IUserService
{
    private readonly INeonDomain _domain;

    public UserService(INeonDomain domain)
    {
        _domain = domain;
    }

    public RegisterResult Guest(string username, out int id)
    {
        //if (_domain.UserRepository.ContainsUsername(username))
        //    return false;

        id = 1;

        //_domain.UserRepository.Add(new User
        //{
        //    Username = username,
        //    Password = id,
        //    Role = UserRole.Guest,
        //    LastActiveAt = DateTime.Now
        //});

        //_domain.SaveChanges();

        return RegisterResult.Success;
    }

    public LoginResult Login(string username, string password, out int id)
    {
        id = 2;

        return LoginResult.Success;
    }

    public RegisterResult Register(string username, string password, out int id)
    {
        id = 2;

        return RegisterResult.Success;
    }
}