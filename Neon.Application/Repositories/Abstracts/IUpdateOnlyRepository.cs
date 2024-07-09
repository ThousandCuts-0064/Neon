using Neon.Domain.Abstracts;

namespace Neon.Application.Repositories.Abstracts;

public interface IUpdateOnlyRepository<in TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public void UpdateById(TEntity entity);
    public void UpdateRangeById(IEnumerable<TEntity> entities);
}