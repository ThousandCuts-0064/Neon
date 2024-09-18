using System.Linq.Expressions;
using Neon.Application.Projections.Bases;
using Neon.Domain.Enums;

namespace Neon.Application.Projections;

public class IncomingUserRequestProjection :
    IJoinProjection<UserRequestPolymorphicProjectionJoinUser, IncomingUserRequestProjection>
{
    public required Guid RequesterKey { get; init; }
    public required string RequesterUsername { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required UserRequestType Type { get; init; }

    public static Expression<Func<UserRequestPolymorphicProjectionJoinUser, IncomingUserRequestProjection>> FromJoin { get; } =
        x => new IncomingUserRequestProjection
        {
            RequesterKey = x.User.Key,
            RequesterUsername = x.User.Username,
            CreatedAt = x.UserRequestPolymorphicProjection.CreatedAt,
            Type = x.UserRequestPolymorphicProjection.Type
        };
}