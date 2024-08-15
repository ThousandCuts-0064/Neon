using System.Security.Claims;

namespace Neon.Web.Utils.Extensions;

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

    public static string GetUsername(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.Name ?? throw new InvalidOperationException();
    }
}