using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities;

namespace Neon.Data.Configuration;

internal class DuelRequestConfiguration : IEntityTypeConfiguration<DuelRequest>
{
    public void Configure(EntityTypeBuilder<DuelRequest> builder)
    {
        builder.HasKey(x => new { x.RequesterUserId, x.ResponderUserId });
    }
}
