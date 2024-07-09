using System.Security.Claims;

namespace Neon.Web.Utils.Extensions;

public static class ClaimsPrincipalEx
{
    public static bool IsAuthenticated(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.IsAuthenticated ?? false;
    }

    public static string GetUsername(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.Name ?? throw new InvalidOperationException();
    }
}