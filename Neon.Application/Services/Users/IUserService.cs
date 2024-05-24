using System.Diagnostics.CodeAnalysis;

namespace Neon.Application.Services.Users;

public interface IUserService
{
    public bool CreateGuest(string username, [NotNullWhen(true)] out string? secret);
    public AuthenticateResult Authenticate(string username, string secret);
}