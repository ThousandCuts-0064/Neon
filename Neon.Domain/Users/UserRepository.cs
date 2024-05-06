using Microsoft.EntityFrameworkCore;
using Neon.Data;
using Neon.Data.Entities;
using Neon.Domain.Abstractions;

namespace Neon.Domain.Users;

internal class UserRepository : Repository<User>, IUserRepository
{
    protected override DbSet<User> DbSet => DbContext.Users;

    public UserRepository(NeonDbContext dbContext) : base(dbContext) { }

    public bool ContainsUsername(string username) => DbSet.Any(x => x.Username == username);
    public User? GetByUsername(string username) => DbSet.FirstOrDefault(x => x.Username == username);
}