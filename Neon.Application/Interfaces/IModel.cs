using System.Linq.Expressions;
using Neon.Application.Projections;
using Neon.Application.Projections.Bases;

namespace Neon.Application.Interfaces;

public interface IModel<TProjection, TSelf>
    where TProjection : IProjection
    where TSelf : IModel<TProjection, TSelf>
{
    public static abstract Expression<Func<TProjection, TSelf>> FromProjection { get; }
}

public interface IUserModel<TSelf> : IModel<UserProjection, TSelf> where TSelf : IUserModel<TSelf>;
public interface IIncomingUserRequestModel<TSelf> : IModel<IncomingUserRequestProjection, TSelf> where TSelf : IIncomingUserRequestModel<TSelf>;
public interface IOutgoingUserRequestModel<TSelf> : IModel<OutgoingUserRequestProjection, TSelf> where TSelf : IOutgoingUserRequestModel<TSelf>;