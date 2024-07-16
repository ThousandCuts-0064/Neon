using Neon.Domain.Entities.Bases;

namespace Neon.Application.Repositories.Bases;

public interface IUpdateOnlyRepository<in TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public void UpdateById(TEntity entity);
    public void UpdateRangeById(IEnumerable<TEntity> entities);
}