using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Users;

namespace Neon.Data.Configuration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(16);
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.Password);
        builder.Property(x => x.LastActiveAt).IsRequired();
    }
}