using Microsoft.AspNetCore.Identity;

namespace Neon.Data.Identity;

public class NeonUser : IdentityUser<int>
{
    public DateTime RegisteredAt { get; set; }
    public DateTime LastActiveAt { get; set; }
}
