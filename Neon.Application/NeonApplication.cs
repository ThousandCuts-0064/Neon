using Neon.Application.Services.Users;
using Neon.Domain;

namespace Neon.Application;

public class NeonApplication : INeonApplication
{
    private readonly INeonDomain _domain;
    public IUserService UserService { get; }

    public NeonApplication(INeonDomain domain)
    {
        _domain = domain;
        UserService = new UserService(_domain);
    }
}