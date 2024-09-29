using System.Linq.Expressions;
using Neon.Application.Interfaces;
using Neon.Application.Projections;

namespace Neon.Web.Models;

public class FriendModel : IUserModel<FriendModel>
{
    public required Guid Key { get; init; }
    public required string Username { get; init; }

    public static Expression<Func<UserProjection, FriendModel>> FromProjection { get; } = x => new FriendModel
    {
        Key = x.Key,
        Username = x.Username
    };
}