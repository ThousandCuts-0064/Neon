using Neon.Data;

namespace Neon.Infrastructure.Services.Bases;

internal class DbContextService
{
    protected NeonDbContext DbContext { get; set; }

    public DbContextService(NeonDbContext dbContext)
    {
        DbContext = dbContext;
    }
}