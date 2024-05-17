namespace Neon.Domain.Abstractions;

public interface IReadableRepository<out TEntity> where TEntity : Entity
{
    public IQueryable<TEntity> GetAll();
    public TEntity? GetById(int id);
}