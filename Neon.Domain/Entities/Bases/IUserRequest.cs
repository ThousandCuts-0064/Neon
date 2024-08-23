namespace Neon.Domain.Entities.Bases;

public interface IUserRequest
{
    public int RequesterUserId { get; set; }
    public int ResponderUserId { get; set; }
}