using Neon.Data.Entities;
using Neon.Domain.Abstractions;

namespace Neon.Domain.Users;

public interface IUserRepository : IStandardRepository<User>;