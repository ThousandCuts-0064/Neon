using System.Linq.Expressions;
using Neon.Application.Models;
using Neon.Application.Projections;

namespace Neon.Web.Models;

public class UserModel : IUserModel<UserModel>
{
    public required string Username { get; init; }

    public static Expression<Func<UserSecureProjection, UserModel>> FromProjection { get; } = x => new UserModel
    {
        Username = x.Username
    };
}