using Neon.Data;

namespace Neon.Infrastructure.Repositories.Abstracts;

internal abstract class DbRepository
{
    protected NeonDbContext DbContext { get; }

    protected DbRepository(NeonDbContext dbContext) => DbContext = dbContext;
}