namespace Neon.Domain.Abstractions;

public interface IAddableRepository<in TEntity> where TEntity : IEntity
{
    public void Add(TEntity entity);
    public void AddRange(IEnumerable<TEntity> entities);

}