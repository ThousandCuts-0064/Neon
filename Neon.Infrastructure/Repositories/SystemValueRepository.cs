using Microsoft.EntityFrameworkCore;
using Neon.Application.Repositories;
using Neon.Data;
using Neon.Domain.Entities;
using Neon.Infrastructure.Repositories.Bases;

namespace Neon.Infrastructure.Repositories;

internal class SystemValueRepository : CrudRepository<SystemValue, string>, ISystemValueRepository
{
    protected override DbSet<SystemValue> DbSet => DbContext.SystemValues;

    public SystemValueRepository(NeonDbContext dbContext) : base(dbContext) { }
}