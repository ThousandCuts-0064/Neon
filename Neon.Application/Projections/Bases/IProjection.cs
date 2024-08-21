using Neon.Domain.Entities.Bases;
using System.Linq.Expressions;

namespace Neon.Application.Projections.Bases;

public interface IProjection;

public interface IProjection<TEntity, TSelf> : IProjection
    where TEntity : IEntity
    where TSelf : IProjection<TEntity, TSelf>
{
    public static abstract Expression<Func<TEntity, TSelf>> FromEntity { get; }
}