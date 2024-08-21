using System.Linq.Expressions;
using Neon.Application.Models;
using Neon.Application.Projections;

namespace Neon.Web.Models;

public class OpponentModel : IOpponentModel<OpponentModel>
{
    public required string Username { get; init; }

    public static Expression<Func<UserSecureProjection, OpponentModel>> FromProjection { get; } = x => new OpponentModel
    {
        Username = x.Username
    };
}