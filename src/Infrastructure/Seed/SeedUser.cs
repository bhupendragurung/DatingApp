namespace Infrastructure.Seed;

public sealed record SeedUser(
    string UserName,
    string KnownAs,
    string Gender,
    DateOnly DateOfBirth,
    string City,
    string Country,
    string? Introduction,
    string? LookingFor,
    string? Interests,
    string[] Roles);