using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities.UserRequests.Bases;

namespace Neon.Data.Configuration.UserRequests.Bases;

internal abstract class UserRequestConfiguration<TUserRequest> : IEntityTypeConfiguration<TUserRequest>
    where TUserRequest : UserRequest
{
    public void Configure(EntityTypeBuilder<TUserRequest> builder)
    {
        builder.HasKey(x => new { RequesterUserId = x.RequesterId, ResponderUserId = x.ResponderId });
    }
}