using System.Linq.Expressions;

namespace Neon.Application.Projections.Bases;

public interface IJoin;

public interface IJoin<TLeft, TRight, TSelf> : IJoin
    where TSelf : IJoin<TLeft, TRight, TSelf>
{
    public static abstract Expression<Func<TLeft, TRight, TSelf>> FromComponents { get; }
}