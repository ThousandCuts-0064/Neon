using System.Security.Claims;
using Neon.Domain.Enums;

namespace Neon.Application.Factories.Principals;

public interface IPrincipalFactory
{
    public ClaimsPrincipal Create(int userId, Guid key, string username, UserRole role, Guid securityKey, Guid identityKey);
}