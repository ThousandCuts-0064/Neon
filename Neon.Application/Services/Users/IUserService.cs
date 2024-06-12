using Microsoft.AspNetCore.Identity;

namespace Neon.Application.Services.Users;

public interface IUserService
{
    public Task<IdentityResult> GuestAsync(string username);
    public Task<LoginResult> LoginAsync(string username, string password);
    public Task<RegisterResult> RegisterAsync(string username, string password);
}