using Neon.Domain.Enums;

namespace Neon.Application.Services.Authentications;

public interface IAuthenticationService
{
    public Task<RegisterResult> GuestAsync(string username, bool rememberMe);
    public Task<LoginResult> LoginAsync(string username, string password, bool rememberMe);
    public Task<RegisterResult> RegisterAsync(string username, string password, bool rememberMe);
    public Task LogoutAsync();
}