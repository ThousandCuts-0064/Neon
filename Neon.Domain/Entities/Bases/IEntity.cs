﻿namespace Neon.Domain.Entities.Bases;

public interface IEntity;

public interface IEntity<TId> : IEntity where TId : IEquatable<TId>
{
    public TId Id { get; init; }
}

public interface IKeyedEntity<TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Key { get; init; }
}