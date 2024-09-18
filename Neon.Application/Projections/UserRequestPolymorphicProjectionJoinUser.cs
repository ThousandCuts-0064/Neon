using System.Linq.Expressions;
using Neon.Application.Projections.Bases;
using Neon.Domain.Entities;

namespace Neon.Application.Projections;

public class UserRequestPolymorphicProjectionJoinUser : IJoin<UserRequestPolymorphicProjection, User, UserRequestPolymorphicProjectionJoinUser>
{
    public required UserRequestPolymorphicProjection UserRequestPolymorphicProjection { get; init; }
    public required User User { get; init; }

    public static Expression<Func<UserRequestPolymorphicProjection, User, UserRequestPolymorphicProjectionJoinUser>> FromComponents { get; } =
        (x, y) => new UserRequestPolymorphicProjectionJoinUser
        {
            UserRequestPolymorphicProjection = x,
            User = y
        };
}