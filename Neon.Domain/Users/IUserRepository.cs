using Neon.Domain.Abstractions;

namespace Neon.Domain.Users;

public interface IUserRepository : IStandardRepository<User>
{
    public bool ContainsUsername(string username);
    public User? GetByUsername(string username);
}