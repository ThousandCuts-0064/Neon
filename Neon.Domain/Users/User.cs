using Neon.Domain.Abstractions;

namespace Neon.Domain.Users;

public class User : Entity
{
    public required string Username { get; set; }
    public required string Secret { get; set; }
    public required UserRole Role { get; set; }
    public required DateTime LastActiveAt { get; set; }
}
