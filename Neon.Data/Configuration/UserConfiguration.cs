using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities;

namespace Neon.Data.Configuration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key).UseIdentityByDefaultColumn();
        builder.Property(x => x.UserName).HasMaxLength(User.USERNAME_MAX_LENGTH);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(User.USERNAME_MAX_LENGTH);

        builder.HasIndex(x => x.Key).IsUnique();
        builder.HasIndex(x => x.UserName).IsUnique();
    }
}