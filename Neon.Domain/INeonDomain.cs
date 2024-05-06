using Neon.Domain.Users;

namespace Neon.Domain;

public interface INeonDomain
{
    public IUserRepository UserRepository { get; }
}