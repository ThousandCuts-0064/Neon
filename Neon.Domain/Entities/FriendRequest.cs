using Neon.Domain.Entities.Bases;

namespace Neon.Domain.Entities;

public class FriendRequest : IUserRequest
{
    public int RequesterUserId { get; set; }
    public int ResponderUserId { get; set; }
}