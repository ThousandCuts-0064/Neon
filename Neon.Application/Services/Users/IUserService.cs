namespace Neon.Application.Services.Users;

public interface IUserService
{
    public bool CreateGuest(string username, out int id);
    public AuthenticateResult Authenticate(string username, string secret);
}