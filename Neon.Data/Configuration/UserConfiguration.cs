using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Users;

namespace Neon.Data.Configuration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(User.USERNAME_MAX_LENGTH);
        builder.Property(x => x.NormalizedUserName).IsRequired().HasMaxLength(User.USERNAME_MAX_LENGTH);
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.RegisteredAt).IsRequired();
        builder.Property(x => x.LastActiveAt).IsRequired();
        builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(User.PasswordHash)}",
            $"""
             "{nameof(User.Role)}" = {(int)UserRole.Guest} AND "{nameof(User.PasswordHash)}" IS NULL
             OR
             "{nameof(User.Role)}" != {(int)UserRole.Guest} AND "{nameof(User.PasswordHash)}" IS NOT NULL
             """));
    }
}