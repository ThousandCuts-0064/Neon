using System.Security.Claims;
using Neon.Domain.Entities;
using Neon.Domain.Enums;

namespace Neon.Application.Factories.Principals;

internal class PrincipalFactory : IPrincipalFactory
{
    public ClaimsPrincipal Create(int userId, Guid key, string username, UserRole role, Guid securityKey, Guid identityKey)
    {
        var claimsIdentity = new ClaimsIdentity("ApplicationCookie");

        claimsIdentity.AddClaim(new Claim(
            ClaimTypes.NameIdentifier,
            userId.ToString(),
            ClaimValueTypes.Integer32,
            null,
            null,
            claimsIdentity));

        claimsIdentity.AddClaim(new Claim(
            nameof(User.Key),
            key.ToString(),
            ClaimValueTypes.String,
            null,
            null,
            claimsIdentity));

        claimsIdentity.AddClaim(new Claim(
            ClaimTypes.Name,
            username,
            ClaimValueTypes.String,
            null,
            null,
            claimsIdentity));

        claimsIdentity.AddClaim(new Claim(
            ClaimTypes.Role,
            role.ToString(),
            ClaimValueTypes.String,
            null,
            null,
            claimsIdentity));

        claimsIdentity.AddClaim(new Claim(
            nameof(User.SecurityKey),
            securityKey.ToString(),
            ClaimValueTypes.String,
            null,
            null,
            claimsIdentity));

        claimsIdentity.AddClaim(new Claim(
            nameof(User.IdentityKey),
            identityKey.ToString(),
            ClaimValueTypes.String,
            null,
            null,
            claimsIdentity));

        return new ClaimsPrincipal(claimsIdentity);
    }
}