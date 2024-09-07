namespace Neon.Domain.Entities.UserRequests.Bases;

public abstract class UserRequest
{
    public int RequesterId { get; init; }
    public int ResponderId { get; init; }
}
