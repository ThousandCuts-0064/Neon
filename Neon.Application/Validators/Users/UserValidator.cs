using System.Text.RegularExpressions;
using Neon.Domain.Entities;

namespace Neon.Application.Validators.Users;

internal partial class UserValidator : IUserValidator
{
    public bool IsUsernameValid(string username) => UsernameRegex().IsMatch(username);
    public bool IsPasswordValid(string password) => PasswordRegex().IsMatch(password);

    [GeneratedRegex(User.USERNAME_REGEX)]
    private static partial Regex UsernameRegex();

    [GeneratedRegex(User.PASSWORD_REGEX)]
    private static partial Regex PasswordRegex();
}