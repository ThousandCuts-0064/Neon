using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities;

namespace Neon.Data.Configuration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(User.USERNAME_MAX_LENGTH);
        builder.Property(x => x.NormalizedUserName).IsRequired().HasMaxLength(User.USERNAME_MAX_LENGTH);
        builder.Property(x => x.RegisteredAt).IsRequired();
        builder.Property(x => x.LastActiveAt).IsRequired();

        builder.HasIndex(x => x.UserName).IsUnique();
    }
}