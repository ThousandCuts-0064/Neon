using Neon.Data;

namespace Neon.Domain.Abstractions;

public interface IRemovableRepository<in TEntity> where TEntity : class, IEntity
{
    public void Remove(TEntity entity);
    public void RemoveRange(IEnumerable<TEntity> entities);

}