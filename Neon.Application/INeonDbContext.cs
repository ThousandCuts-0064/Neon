using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Neon.Domain.Entities;
using Neon.Domain.Notifications.Bases;

namespace Neon.Application;

public interface INeonDbContext
{
    public DatabaseFacade Database { get; }

    public DbSet<SystemValue> SystemValues { get; }
    public DbSet<User> Users { get; }
    public DbSet<IdentityUserRole<int>> UserRoles { get; }
    public DbSet<IdentityRole<int>> Roles { get; }
    public DbSet<FriendRequest> FriendRequests { get; }
    public DbSet<TradeRequest> TradeRequests { get; }
    public DbSet<DuelRequest> DuelRequests { get; }

    public Task NotifyAsync<T>(T payload) where T : Notification;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}