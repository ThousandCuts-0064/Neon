using System.Diagnostics.CodeAnalysis;
using Neon.Domain.Entities.Bases;
using Neon.Domain.Enums;

namespace Neon.Domain.Entities;

public class User : KeyedEntity<int, Guid>
{
    public const int USERNAME_MAX_LENGTH = 16;
    public const int USERNAME_MIN_LENGTH = 4;

    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string USERNAME_REGEX = "^[a-zA-Z0-9-_]+$";

    public const int PASSWORD_MIN_LENGTH = 8;

    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string PASSWORD_REGEX = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$";

    public required string Username { get; init; }
    public string? PasswordHash { get; init; }
    public required UserRole Role { get; init; }
    public required DateTime RegisteredAt { get; init; }
    public required DateTime LastActiveAt { get; init; }
    public string? ConnectionId { get; init; }

    /// <summary>
    /// Updated every time <see cref="PasswordHash"/> changes.
    /// </summary>
    public required Guid SecurityKey { get; init; }

    /// <summary>
    /// Updated every time <see cref="Username"/> or <see cref="Role"/> changes.
    /// </summary>
    public required Guid IdentityKey { get; init; }
}