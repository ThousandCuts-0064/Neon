using Neon.Web.Models;

namespace Neon.Web.Args.Client;

public class InitializeArgs
{
    public required IReadOnlyCollection<ActiveUserModel> ActiveUsers { get; init; }
    public required IReadOnlyCollection<FriendModel> Friends { get; init; }
    public required IReadOnlyCollection<IncomingUserRequestModel> IncomingUserRequests { get; init; }
    public required IReadOnlyCollection<OutgoingUserRequestModel> OutgoingUserRequests { get; init; }
}
