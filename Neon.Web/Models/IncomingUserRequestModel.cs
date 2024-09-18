using System.Linq.Expressions;
using Neon.Application.Interfaces;
using Neon.Application.Projections;
using Neon.Domain.Enums;

namespace Neon.Web.Models;

public class IncomingUserRequestModel : IIncomingUserRequestModel<IncomingUserRequestModel>
{
    public required UserRequestType Type { get; init; }
    public required Guid RequesterKey { get; init; }
    public required string RequesterUsername { get; init; }
    public required DateTime CreatedAt { get; init; }

    public static Expression<Func<IncomingUserRequestProjection, IncomingUserRequestModel>> FromProjection => x =>
        new IncomingUserRequestModel
        {
            Type = x.Type,
            RequesterKey = x.RequesterKey,
            RequesterUsername = x.RequesterUsername,
            CreatedAt = x.CreatedAt
        };
}