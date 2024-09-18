using System.Linq.Expressions;
using Neon.Application.Projections.Bases;
using Neon.Domain.Enums;

namespace Neon.Application.Projections;

public class OutgoingUserRequestProjection :
    IJoinProjection<UserRequestPolymorphicProjectionJoinUser, OutgoingUserRequestProjection>
{
    public required UserRequestType Type { get; init; }
    public required Guid ResponderKey { get; init; }
    public required string ResponderUsername { get; init; }
    public required DateTime CreatedAt { get; init; }

    public static Expression<Func<UserRequestPolymorphicProjectionJoinUser, OutgoingUserRequestProjection>> FromJoin { get; } =
        x => new OutgoingUserRequestProjection
        {
            Type = x.UserRequestPolymorphicProjection.Type,
            ResponderKey = x.User.Key,
            ResponderUsername = x.User.Username,
            CreatedAt = x.UserRequestPolymorphicProjection.CreatedAt
        };
}