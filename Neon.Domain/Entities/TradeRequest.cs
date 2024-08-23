using Neon.Domain.Entities.Bases;

namespace Neon.Domain.Entities;

public class TradeRequest : IUserRequest
{
    public int RequesterUserId { get; set; }
    public int ResponderUserId { get; set; }
}