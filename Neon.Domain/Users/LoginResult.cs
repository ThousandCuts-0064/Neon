namespace Neon.Domain.Users;

public enum LoginResult
{
    Error,
    UsernameNotFound,
    WrongPassword,
    Success
}