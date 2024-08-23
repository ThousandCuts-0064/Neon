using Neon.Domain.Enums;

namespace Neon.Web.Args.Client;

public abstract class MessageArgs
{
    public required string UsernamePrefix { get; init; }
    public required string Username { get; init; }
    public required string UsernameSuffix { get; init; }
    public required string Message { get; init; }

}

public class UserMessageArgs : MessageArgs
{
    public required UserRole UserRole { get; init; }
    public required bool IsImportant { get; init; }
}

public class CommandMessageArgs : MessageArgs;
