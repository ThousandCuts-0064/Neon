using Neon.Data.Enums;

namespace Neon.Data.Entities;

public class User : IEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Secret { get; set; }
    public UserRole Role { get; set; }
    public DateTime LastlyActiveAt { get; set; }
}
