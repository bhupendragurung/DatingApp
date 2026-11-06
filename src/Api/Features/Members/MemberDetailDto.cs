namespace Api.Features.Members;

public sealed record MemberDetailDto(
    Guid Id,
    string UserName,
    string KnownAs,
    int Age,
    string Gender,
    string City,
    string Country,
    DateTime CreatedAt,
    string? Introduction,
    string? LookingFor,
    string? Interests);