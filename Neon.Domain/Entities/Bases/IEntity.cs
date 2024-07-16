namespace Neon.Domain.Entities.Bases;

public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}

public interface IEntity : IEntity<int>;