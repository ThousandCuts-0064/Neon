using Neon.Domain.Users;

namespace Neon.Web.Models;

public class UserModel
{
    public required string Username { get; set; }
    public required UserRole Role { get; set; }
    public DateTime? LastActiveAt { get; set; }
}