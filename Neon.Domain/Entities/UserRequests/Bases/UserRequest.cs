using Neon.Domain.Entities.Bases;

namespace Neon.Domain.Entities.UserRequests.Bases;

public abstract class UserRequest : IEntity
{
    public int RequesterId { get; init; }
    public int ResponderId { get; init; }
    public DateTime CreatedAt { get; init; }
}
