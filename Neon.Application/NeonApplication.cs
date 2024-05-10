using Neon.Domain;

namespace Neon.Application;

public class NeonApplication : INeonApplication
{
    private readonly INeonDomain _domain;

    public NeonApplication(INeonDomain domain)
    {
        _domain = domain;
    }
}