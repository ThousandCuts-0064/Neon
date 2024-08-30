namespace Neon.Web.Args.Client;

public class UserConnectionToggledArgs
{
    public required string Username { get; init; }
    public bool IsActive { get; init; }
}
