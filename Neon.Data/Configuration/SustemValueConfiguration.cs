using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities;

namespace Neon.Data.Configuration;

internal class SustemValueConfiguration : IEntityTypeConfiguration<SystemValue>
{
    public void Configure(EntityTypeBuilder<SystemValue> builder)
    {
        builder.HasKey(x => x.Id);
    }
}