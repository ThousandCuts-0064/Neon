using System.Linq.Expressions;
using Neon.Application.Projections.Bases;
using Neon.Domain.Entities;

namespace Neon.Application.Projections;

public class UserSecureProjection : IProjection<User, UserSecureProjection>
{
    public required Guid Key { get; init; }
    public required string Username { get; init; }

    public static Expression<Func<User, UserSecureProjection>> FromEntity { get; } = x => new UserSecureProjection
    {
        Key = x.Key,
        Username = x.Username
    };
}