using Neon.Application.Interfaces;

namespace Neon.Application.Services.Bases;

internal abstract class DbContextService
{
    protected INeonDbContext DbContext { get; }

    protected DbContextService(INeonDbContext dbContext)
    {
        DbContext = dbContext;
    }
}