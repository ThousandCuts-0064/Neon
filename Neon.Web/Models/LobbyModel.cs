namespace Neon.Web.Models;

public class LobbyModel
{
    public required UserModel User { get; init; }
    //public required IReadOnlyCollection<ActiveUserModel> ActiveUsers { get; init; }
    //public required IReadOnlyCollection<UserModel> Friends { get; init; }
    //public required IReadOnlyCollection<IncomingUserRequestModel> IncomingUserRequests { get; init; }
    //public required IReadOnlyCollection<OutgoingUserRequestModel> OutgoingUserRequests { get; init; }
}