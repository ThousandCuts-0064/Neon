using Neon.Data;
using Neon.Domain.Users;

namespace Neon.Domain;

public class NeonDomain : INeonDomain
{
    public IUserRepository UserRepository { get; }

    public NeonDomain(NeonDbContext dbContext)
    {
        UserRepository = new UserRepository(dbContext);
    }
}