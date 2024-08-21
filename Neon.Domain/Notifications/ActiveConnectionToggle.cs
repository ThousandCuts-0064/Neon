﻿using Neon.Domain.Notifications.Bases;

namespace Neon.Domain.Notifications;

public class ActiveConnectionToggle : Notification
{
    public required string UserName { get; init; }
    public required bool IsActive { get; init; }
}
