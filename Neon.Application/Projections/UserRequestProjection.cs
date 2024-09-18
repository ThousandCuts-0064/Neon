using System.Linq.Expressions;
using Neon.Application.Projections.Bases;
using Neon.Domain.Entities.UserRequests;
using Neon.Domain.Entities.UserRequests.Bases;
using Neon.Domain.Enums;

namespace Neon.Application.Projections;

public class UserRequestProjection : IProjection<UserRequest, UserRequestProjection>
{
    public required int RequesterId { get; init; }
    public required int ResponderId { get; init; }
    public required DateTime CreatedAt { get; init; }

    public static Expression<Func<UserRequest, UserRequestProjection>> FromEntity { get; } = x =>
        new UserRequestProjection
        {
            RequesterId = x.RequesterId,
            ResponderId = x.ResponderId,
            CreatedAt = x.CreatedAt
        };
}

public class UserRequestPolymorphicProjection : UserRequestProjection
{
    public required UserRequestType Type { get; init; }
}

public class DuelRequestProjection : UserRequestPolymorphicProjection,
    IPolymorphicProjection<DuelRequest, UserRequestPolymorphicProjection, UserRequest, UserRequestProjection>
{
    public static Expression<Func<DuelRequest, UserRequestPolymorphicProjection>> BaseFromEntity { get; } = x =>
        new UserRequestPolymorphicProjection
        {
            RequesterId = x.RequesterId,
            ResponderId = x.ResponderId,
            CreatedAt = x.CreatedAt,
            Type = UserRequestType.Duel
        };
}

public class TradeRequestProjection : UserRequestPolymorphicProjection,
    IPolymorphicProjection<TradeRequest, UserRequestPolymorphicProjection, UserRequest, UserRequestProjection>
{
    public static Expression<Func<TradeRequest, UserRequestPolymorphicProjection>> BaseFromEntity { get; } = x =>
        new UserRequestPolymorphicProjection
        {
            RequesterId = x.RequesterId,
            ResponderId = x.ResponderId,
            CreatedAt = x.CreatedAt,
            Type = UserRequestType.Trade
        };
}

public class FriendRequestProjection : UserRequestPolymorphicProjection,
    IPolymorphicProjection<FriendRequest, UserRequestPolymorphicProjection, UserRequest, UserRequestProjection>
{
    public static Expression<Func<FriendRequest, UserRequestPolymorphicProjection>> BaseFromEntity { get; } = x =>
        new UserRequestPolymorphicProjection
        {
            RequesterId = x.RequesterId,
            ResponderId = x.ResponderId,
            CreatedAt = x.CreatedAt,
            Type = UserRequestType.Friend
        };
}