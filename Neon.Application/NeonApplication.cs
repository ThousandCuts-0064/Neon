using Microsoft.AspNetCore.Identity;
using Neon.Application.Services.Users;
using Neon.Domain;
using Neon.Domain.Users;

namespace Neon.Application;

public class NeonApplication : INeonApplication
{
    private readonly INeonDomain _domain;
    public IUserService UserService { get; }

    public NeonApplication(INeonDomain domain, UserManager<User> userManager)
    {
        _domain = domain;
        UserService = new UserService(_domain, userManager);
    }
}