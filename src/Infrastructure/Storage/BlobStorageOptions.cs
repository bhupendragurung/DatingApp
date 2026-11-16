using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Storage;

public sealed class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;

    [Required]
    public string ContainerName { get; init; } = string.Empty;
}