using System.Linq.Expressions;
using Neon.Application;
using Neon.Application.Interfaces;
using Neon.Application.Projections;

namespace Neon.Web.Models;

public class UserModel : IUserModel<UserModel>
{
    public required string Username { get; init; }

    public static Expression<Func<UserProjection, UserModel>> FromProjection { get; } = x => new UserModel
    {
        Username = x.Username
    };
}