namespace Neon.Application.Services.Users;

public interface IUserService
{
    public RegisterResult Guest(string username, out int id);
    public LoginResult Login(string username, string password, out int id);
    public RegisterResult Register(string username, string password, out int id);
}