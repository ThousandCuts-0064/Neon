using Microsoft.EntityFrameworkCore;
using Neon.Data.Core;
using Neon.Domain.Users;

namespace Neon.Infrastructure.Repositories;

internal class UserRepository : Repository<User>, IUserRepository
{
    protected override DbSet<User> DbSet => DbContext.Users;

    public UserRepository(NeonDbContext dbContext) : base(dbContext) { }

    public bool ContainsUsername(string username) => DbSet.Any(x => x.Username == username);
    public User? GetByUsername(string username) => DbSet.FirstOrDefault(x => x.Username == username);
}