using Neon.Domain;
using Neon.Domain.Users;

namespace Neon.Application;

public class NeonApplication : INeonApplication
{
    private readonly INeonDomain _domain;

    public IUserRepository UserRepository => _domain.UserRepository;

    public NeonApplication(INeonDomain domain)
    {
        _domain = domain;
    }
}