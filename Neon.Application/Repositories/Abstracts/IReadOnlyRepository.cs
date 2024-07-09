using Neon.Domain.Abstracts;

namespace Neon.Application.Repositories.Abstracts;

public interface IReadOnlyRepository<TEntity, in TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public IQueryable<TEntity> All { get; }
    public IQueryable<TEntity> FilterById(TKey id);
    public Task<TEntity?> FindByIdAsync(TKey id);
    public Task<TEntity> FindRequiredByIdAsync(TKey id);
}