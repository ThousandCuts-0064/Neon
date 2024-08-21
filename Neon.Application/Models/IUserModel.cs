using Neon.Application.Models.Bases;
using Neon.Application.Projections;

namespace Neon.Application.Models;

public interface IUserModel<TSelf> : IModel<UserSecureProjection, TSelf> where TSelf : IUserModel<TSelf>;