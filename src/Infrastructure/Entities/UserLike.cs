using Infrastructure.Identity;

namespace Infrastructure.Entities;

public class UserLike
{
    public Guid SourceUserId { get; set; }
    public AppUser SourceUser { get; set; } = null!;

    public Guid TargetUserId { get; set; }
    public AppUser TargetUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}