using Neon.Data.Enums;

namespace Neon.Web.Models;

public abstract class UserModel
{
    public string Username { get; set; }
    public string Secret { get; set; }
    public abstract UserRole Role { get; }
    public DateTime? LastActiveAt { get; set; }
}