namespace Neon.Domain.Entities.Bases;

public abstract class Entity<TId> : IEntity<TId> where TId : IEquatable<TId>
{
    public TId Id { get; init; } = default!;
}

public abstract class KeyedEntity<TId, TKey> : Entity<TId>, IKeyedEntity<TKey>
    where TId : IEquatable<TId>
    where TKey : IEquatable<TKey>
{
    public TKey Key { get; init; } = default!;
}