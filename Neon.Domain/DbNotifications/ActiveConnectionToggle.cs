using Neon.Domain.DbNotifications.Bases;

namespace Neon.Domain.DbNotifications;

public class ActiveConnectionToggle : DbNotification
{
    public string UserName { get; set; } = null!;
    public bool IsActive { get; set; }
}
