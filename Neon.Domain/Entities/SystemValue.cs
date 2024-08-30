using Neon.Domain.Entities.Bases;

namespace Neon.Domain.Entities;

public class SystemValue : Entity<string>
{
    public required string Value { get; init; }
}
