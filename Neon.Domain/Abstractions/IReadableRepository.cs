using Neon.Data;

namespace Neon.Domain.Abstractions;

public interface IReadableRepository<out TEntity> where TEntity : class, IEntity
{
    public IQueryable<TEntity> GetAll();
    public TEntity? GetById(int id);
}