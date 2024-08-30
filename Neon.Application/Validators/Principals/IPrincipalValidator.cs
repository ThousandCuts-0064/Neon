using Microsoft.AspNetCore.Authentication.Cookies;

namespace Neon.Application.Validators.Principals;

public interface IPrincipalValidator
{
    public Task ValidateAsync(CookieValidatePrincipalContext principalContext);
}