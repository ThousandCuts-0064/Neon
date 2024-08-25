using System.Linq.Expressions;
using Neon.Application.Models;
using Neon.Application.Projections;

namespace Neon.Web.Models;

public class ActiveUserModel : IUserModel<ActiveUserModel>
{
    public required string Username { get; init; }

    public static Expression<Func<UserSecureProjection, ActiveUserModel>> FromProjection { get; } = x => new ActiveUserModel
    {
        Username = x.Username
    };
}