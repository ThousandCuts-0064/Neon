using Neon.Application.Models.Bases;
using Neon.Application.Projections;

namespace Neon.Application.Models;

public interface IOpponentModel<TSelf> : IModel<UserSecureProjection, TSelf> where TSelf : IOpponentModel<TSelf>;