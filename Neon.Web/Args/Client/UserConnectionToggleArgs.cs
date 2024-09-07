namespace Neon.Web.Args.Client;

public class UserConnectionToggledArgs
{
    public required Guid Key { get; init; }
    public required string Username { get; init; }
    public required bool IsActive { get; init; }
}
