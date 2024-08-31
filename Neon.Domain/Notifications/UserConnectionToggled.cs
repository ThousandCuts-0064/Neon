﻿using Neon.Domain.Notifications.Bases;

namespace Neon.Domain.Notifications;

public class UserConnectionToggled : Notification
{
    public required string Username { get; init; }
    public required bool IsActive { get; init; }
}