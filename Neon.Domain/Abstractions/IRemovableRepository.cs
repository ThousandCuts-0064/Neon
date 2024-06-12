namespace Neon.Domain.Abstractions;

public interface IRemovableRepository<in TEntity> where TEntity : IEntity
{
    public void Remove(TEntity entity);
    public void RemoveRange(IEnumerable<TEntity> entities);

}