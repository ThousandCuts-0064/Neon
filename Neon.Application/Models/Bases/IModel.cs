using System.Linq.Expressions;
using Neon.Application.Projections.Bases;

namespace Neon.Application.Models.Bases;

public interface IModel<TProjection, TSelf>
    where TProjection : IProjection
    where TSelf : IModel<TProjection, TSelf>
{
    public static abstract Expression<Func<TProjection, TSelf>> FromProjection { get; }
}