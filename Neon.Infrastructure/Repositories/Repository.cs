using Microsoft.EntityFrameworkCore;
using Neon.Data;
using Neon.Domain.Abstractions;

namespace Neon.Infrastructure.Repositories;

internal abstract class Repository<TEntity> where TEntity : Entity
{
    protected NeonDbContext DbContext { get; }
    protected abstract DbSet<TEntity> DbSet { get; }

    protected Repository(NeonDbContext dbContext) => DbContext = dbContext;

    public virtual IQueryable<TEntity> GetAll() => DbSet;
    public virtual TEntity? GetById(int id) => DbSet.FirstOrDefault(x => x.Id == id);
    public virtual void Add(TEntity entity) => DbSet.Add(entity);
    public virtual void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);
    public virtual void Update(TEntity entity) => DbSet.Update(entity);
    public virtual void UpdateRange(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);
    public virtual void Remove(TEntity entity) => DbSet.Remove(entity);
    public virtual void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
}