namespace Neon.Web.Args.Client;

public class ConnectionToggleArgs
{
    public required string Username { get; init; }
    public bool IsActive { get; init; }
}
