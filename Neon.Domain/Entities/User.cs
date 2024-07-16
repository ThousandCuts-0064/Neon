using Microsoft.AspNetCore.Identity;
using Neon.Domain.Entities.Bases;

namespace Neon.Domain.Entities;

public class User : IdentityUser<int>, IEntity
{
    public const int USERNAME_MAX_LENGTH = 16;
    public const int USERNAME_MIN_LENGTH = 4;
    public const int PASSWORD_MIN_LENGTH = 4;

    public new required string UserName
    {
        get => base.UserName!;
        set => base.UserName = value;
    }

    public DateTime RegisteredAt { get; set; }
    public DateTime LastActiveAt { get; set; }
    public string? ActiveConnectionId { get; set; }
}