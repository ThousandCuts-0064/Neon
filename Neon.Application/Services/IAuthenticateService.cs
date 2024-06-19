using Neon.Domain.Users;

namespace Neon.Application.Services;

public interface IAuthenticateService
{
    public Task<RegisterResult> GuestAsync(string username, bool rememberMe);
    public Task<LoginResult> LoginAsync(string username, string password, bool rememberMe);
    public Task<RegisterResult> RegisterAsync(string username, string password, bool rememberMe);
}