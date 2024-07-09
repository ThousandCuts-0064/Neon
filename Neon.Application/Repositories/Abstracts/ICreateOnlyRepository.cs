using Neon.Domain.Abstracts;

namespace Neon.Application.Repositories.Abstracts;

public interface ICreateOnlyRepository<in TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public void Create(TEntity entity);
    public void CreateRange(IEnumerable<TEntity> entities);
}