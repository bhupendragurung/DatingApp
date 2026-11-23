using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
{
    public void Configure(EntityTypeBuilder<UserLike> builder)
    {
        builder.ToTable("Likes");

        builder.HasKey(l => new { l.SourceUserId, l.TargetUserId });

        builder.HasOne(l => l.SourceUser)
            .WithMany(u => u.LikedUsers)
            .HasForeignKey(l => l.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.TargetUser)
            .WithMany(u => u.LikedByUsers)
            .HasForeignKey(l => l.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}