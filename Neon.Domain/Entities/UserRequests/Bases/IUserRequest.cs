namespace Neon.Domain.Entities.UserRequests.Bases;

public interface IUserRequest
{
    public int RequesterId { get; init; }
    public int ResponderId { get; init; }
}