namespace Neon.Domain.Entities.UserRequests.Bases;

public class UserRequest : IUserRequest
{
    public int RequesterId { get; init; }
    public int ResponderId { get; init; }
}
