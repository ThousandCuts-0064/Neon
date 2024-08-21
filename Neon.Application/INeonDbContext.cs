using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Neon.Domain.Entities;

namespace Neon.Application;

public interface INeonDbContext
{
    public DatabaseFacade Database { get; }

    public DbSet<SystemValue> SystemValues { get; }
    public DbSet<User> Users { get; }
    public DbSet<IdentityUserRole<int>> UserRoles { get; }
    public DbSet<IdentityRole<int>> Roles { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}