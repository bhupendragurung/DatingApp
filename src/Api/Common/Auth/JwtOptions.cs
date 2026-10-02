using System.ComponentModel.DataAnnotations;

namespace Api.Common.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required, MinLength(64)]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int ExpiryMinutes { get; init; }
}