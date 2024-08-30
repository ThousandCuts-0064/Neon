namespace Neon.Application.Validators.Users;

public interface IUserValidator
{
    public bool IsUsernameValid(string username);
    public bool IsPasswordValid(string password);
}