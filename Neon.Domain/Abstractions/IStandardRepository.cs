﻿namespace Neon.Domain.Abstractions;

public interface IStandardRepository<TEntity> :
    IReadableRepository<TEntity>,
    IAddableRepository<TEntity>,
    IUpdatableRepository<TEntity>,
    IRemovableRepository<TEntity>
    where TEntity : IEntity;