﻿namespace Neon.Domain.Abstractions;

public interface IUpdatableRepository<in TEntity> where TEntity : IEntity
{
    public void Update(TEntity entity);
    public void UpdateRange(IEnumerable<TEntity> entities);

}