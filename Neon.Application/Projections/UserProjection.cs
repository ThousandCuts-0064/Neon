using System.Linq.Expressions;
using Neon.Application.Projections.Bases;
using Neon.Domain.Entities;

namespace Neon.Application.Projections;

public class UserProjection : IProjection<User, UserProjection>
{
    public required Guid Key { get; init; }
    public required string Username { get; init; }

    public static Expression<Func<User, UserProjection>> FromEntity { get; } = x => new UserProjection
    {
        Key = x.Key,
        Username = x.Username
    };
}