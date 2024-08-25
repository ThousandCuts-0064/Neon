using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities;

namespace Neon.Data.Configuration;

internal class TradeRequestConfiguration : IEntityTypeConfiguration<TradeRequest>
{
    public void Configure(EntityTypeBuilder<TradeRequest> builder)
    {
        builder.HasKey(x => new { x.RequesterUserId, x.ResponderUserId });
    }
}
