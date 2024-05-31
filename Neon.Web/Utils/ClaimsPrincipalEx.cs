    using System.Security.Claims;

namespace Neon.Web.Utils;

public static class ClaimsPrincipalEx
{
    public static bool IsAuthenticated(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.IsAuthenticated ?? false;
    }
}
