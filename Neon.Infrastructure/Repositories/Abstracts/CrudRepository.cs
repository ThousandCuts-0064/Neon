using Microsoft.EntityFrameworkCore;
using Neon.Application.Repositories.Abstracts;
using Neon.Data;
using Neon.Domain.Abstracts;

namespace Neon.Infrastructure.Repositories.Abstracts;

internal abstract class CrudRepository<TEntity, TKey> : DbRepository,
    ICreateOnlyRepository<TEntity, TKey>,
    IReadOnlyRepository<TEntity, TKey>,
    IUpdateOnlyRepository<TEntity, TKey>,
    IDeleteOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected abstract DbSet<TEntity> DbSet { get; }

    protected CrudRepository(NeonDbContext dbContext) : base(dbContext) { }

    public virtual IQueryable<TEntity> All => DbSet;

    public IQueryable<TEntity> FilterById(TKey id) => DbSet.Where(x => x.Id.Equals(id));
    public virtual async Task<TEntity?> FindByIdAsync(TKey id) => await DbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
    public virtual async Task<TEntity> FindRequiredByIdAsync(TKey id) => await DbSet.FirstAsync(x => x.Id.Equals(id));
    public virtual void Create(TEntity entity) => DbSet.Add(entity);
    public virtual void CreateRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);
    public virtual void UpdateById(TEntity entity) => DbSet.Update(entity);
    public virtual void UpdateRangeById(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);
    public virtual void RemoveById(TEntity entity) => DbSet.Remove(entity);
    public virtual void RemoveRangeById(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

}