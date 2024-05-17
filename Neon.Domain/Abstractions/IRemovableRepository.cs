﻿namespace Neon.Domain.Abstractions;

public interface IRemovableRepository<in TEntity> where TEntity : Entity
{
    public void Remove(TEntity entity);
    public void RemoveRange(IEnumerable<TEntity> entities);

}