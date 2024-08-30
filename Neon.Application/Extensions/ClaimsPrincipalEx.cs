using System.Security.Claims;
using Neon.Domain.Entities;
using Neon.Domain.Enums;

namespace Neon.Application.Extensions;

public static class ClaimsPrincipalEx
{
    public static bool IsAuthenticated(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.IsAuthenticated ?? false;
    }

    public static int GetId(this ClaimsPrincipal claimsPrincipal)
    {
        return int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ??
            throw new InvalidOperationException());
    }

    public static Guid GetKey(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.FindFirstValue(nameof(User.Key)) ??
            throw new InvalidOperationException());
    }

    public static string GetUsername(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.Name ?? throw new InvalidOperationException();
    }

    public static UserRole GetRole(this ClaimsPrincipal claimsPrincipal)
    {
        return Enum.Parse<UserRole>(claimsPrincipal.FindFirstValue(ClaimTypes.Role) ??
            throw new InvalidOperationException());
    }

    public static Guid GetSecurityKey(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.FindFirstValue(nameof(User.SecurityKey)) ??
            throw new InvalidOperationException());
    }

    public static Guid GetIdentityKey(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.FindFirstValue(nameof(User.IdentityKey)) ??
            throw new InvalidOperationException());
    }
}