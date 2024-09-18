﻿using System.Linq.Expressions;
using Neon.Application.Interfaces;
using Neon.Application.Projections;
using Neon.Domain.Enums;

namespace Neon.Web.Models;

public class OutgoingUserRequestModel : IOutgoingUserRequestModel<OutgoingUserRequestModel>
{
    public required Guid ResponderKey { get; init; }
    public required string ResponderUsername { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required UserRequestType Type { get; init; }

    public static Expression<Func<OutgoingUserRequestProjection, OutgoingUserRequestModel>> FromProjection => x =>
        new OutgoingUserRequestModel
        {
            ResponderKey = x.ResponderKey,
            ResponderUsername = x.ResponderUsername,
            CreatedAt = x.CreatedAt,
            Type = x.Type
        };
}