using Neon.Domain.Entities.Bases;

namespace Neon.Application.Repositories.Bases;

public interface IDeleteOnlyRepository<in TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public void RemoveById(TEntity entity);
    public void RemoveRangeById(IEnumerable<TEntity> entities);
}