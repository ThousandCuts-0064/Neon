using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Neon.Application.Extensions;
using Neon.Application.Factories.Principals;
using Neon.Application.Interfaces;
using Neon.Application.Services.Bases;

namespace Neon.Application.Validators.Principals;

internal class PrincipalValidator : DbContextService, IPrincipalValidator
{
    private readonly IPrincipalFactory _principalFactory;

    public PrincipalValidator(INeonDbContext dbContext, IPrincipalFactory principalFactory) : base(dbContext)
    {
        _principalFactory = principalFactory;
    }

    public async Task ValidateAsync(CookieValidatePrincipalContext principalContext)
    {
        var userId = principalContext.Principal!.GetId();
        var securityKey = principalContext.Principal!.GetSecurityKey();

        var isSecure = await DbContext.Users
            .Where(x => x.Id == userId && x.SecurityKey == securityKey)
            .AnyAsync();

        if (!isSecure)
        {
            principalContext.ShouldRenew = true;
            principalContext.RejectPrincipal();

            return;
        }

        var identityKey = principalContext.Principal!.GetIdentityKey();

        var user = await DbContext.Users
            .Where(x => x.Id == userId && x.IdentityKey != identityKey)
            .Select(x => new
            {
                x.Username,
                x.Role,
                x.IdentityKey
            })
            .FirstOrDefaultAsync();

        if (user is null)
            return;

        var key = principalContext.Principal!.GetKey();

        principalContext.ShouldRenew = true;
        principalContext.ReplacePrincipal(_principalFactory.Create(
            userId,
            key,
            user.Username,
            user.Role,
            securityKey,
            user.IdentityKey));
    }
}