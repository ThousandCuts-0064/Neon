using Neon.Domain.Abstracts;

namespace Neon.Domain.Entities;

public class SystemValue : IEntity<string>
{
    public const string LAST_ACTIVE_AT = "LastActiveAt";

    public required string Id { get; set; }
    public required string Value { get; set; }
}
