namespace Neon.Domain.Enums;

public enum LoginResult
{
    Error,
    UsernameNotFound,
    CannotLogInGuest,
    WrongPassword,
    Success
}