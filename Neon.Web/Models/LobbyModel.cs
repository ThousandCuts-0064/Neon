namespace Neon.Web.Models;

public class LobbyModel
{
    public required UserModel User { get; init; }
    public required IReadOnlyCollection<ActiveUserModel> ActiveUsers { get; init; }
}