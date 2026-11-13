using Infrastructure.Identity;

namespace Infrastructure.Entities;

public class Photo
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public required string BlobName { get; set; }
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}