using Neon.Domain.Entities.Bases;

namespace Neon.Application.Repositories.Bases;

public interface ICreateOnlyRepository<in TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public void Create(TEntity entity);
    public void CreateRange(IEnumerable<TEntity> entities);
}