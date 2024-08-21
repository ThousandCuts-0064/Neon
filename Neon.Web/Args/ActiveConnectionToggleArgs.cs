namespace Neon.Web.Args;

public class ActiveConnectionToggleArgs
{
    public required string Username { get; init; }
    public bool IsActive { get; init; }
}
