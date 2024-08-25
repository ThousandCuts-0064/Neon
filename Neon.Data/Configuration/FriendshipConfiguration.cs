using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neon.Domain.Entities;

namespace Neon.Data.Configuration;

internal class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(x => new { x.User1Id, x.User2Id });

        builder.ToTable(x => x.HasCheckConstraint("CK_Friendship_UserId1_LessThan_UserId2", "[UserId1] < [UserId2]"));
    }
}