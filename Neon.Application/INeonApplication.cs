using Neon.Application.Services.Users;

namespace Neon.Application;

public interface INeonApplication
{
    public IUserService UserService { get; }
}