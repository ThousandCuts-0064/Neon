namespace Neon.Domain.Entities.Bases;

public interface IEntity;

public interface IKeyedEntity<TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Key { get; init; }
}