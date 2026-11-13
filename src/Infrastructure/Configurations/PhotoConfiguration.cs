using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.Property(p => p.Url).HasMaxLength(2048);
        builder.Property(p => p.BlobName).HasMaxLength(200);

        builder.HasOne(p => p.AppUser)
            .WithMany(u => u.Photos)
            .HasForeignKey(p => p.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}