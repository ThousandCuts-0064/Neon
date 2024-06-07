using Neon.Data.Core;
using Neon.Domain;
using Neon.Domain.Users;
using Neon.Infrastructure.Repositories;

namespace Neon.Infrastructure;

internal class NeonDomain : INeonDomain
{
    private readonly NeonDbContext _dbContext;
    public IUserRepository UserRepository { get; }

    public NeonDomain(NeonDbContext dbContext)
    {
        _dbContext = dbContext;
        UserRepository = new UserRepository(_dbContext);
    }

    public void SaveChanges() => _dbContext.SaveChanges();
}