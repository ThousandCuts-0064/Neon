﻿using Microsoft.AspNetCore.Identity;
using Neon.Domain.Abstractions;

namespace Neon.Domain.Users;

public class User : IdentityUser<int>, IEntity
{
    public const int USERNAME_MAX_LENGTH = 16;
    public const int PASSWORD_MIN_LENGTH = 4;
    public DateTime RegisteredAt { get; set; }
    public DateTime LastActiveAt { get; set; }
}
