using Domain.Auth;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.HasData(
            new AppRole
            {
                Id = Guid.Parse("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e01"),
                Name = Roles.Member,
                NormalizedName = "MEMBER",
                ConcurrencyStamp = "7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e01"
            },
            new AppRole
            {
                Id = Guid.Parse("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e02"),
                Name = Roles.Admin,
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e02"
            },
            new AppRole
            {
                Id = Guid.Parse("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e03"),
                Name = Roles.Moderator,
                NormalizedName = "MODERATOR",
                ConcurrencyStamp = "7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e03"
            });
    }
}