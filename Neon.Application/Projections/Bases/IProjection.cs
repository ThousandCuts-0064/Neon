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

public interface IPolymorphicProjection<TEntity, TProjection, TBaseEntity, TBaseProjection> :
    IProjection<TBaseEntity, TBaseProjection>
    where TEntity : TBaseEntity
    where TProjection : TBaseProjection
    where TBaseEntity : IEntity
    where TBaseProjection : IProjection<TBaseEntity, TBaseProjection>

{
    public static abstract Expression<Func<TEntity, TProjection>> BaseFromEntity { get; }
}

public interface IJoinProjection<TJoin, TSelf> : IProjection
    where TJoin : IJoin
    where TSelf : IJoinProjection<TJoin, TSelf>
{
    public static abstract Expression<Func<TJoin, TSelf>> FromJoin { get; }
}