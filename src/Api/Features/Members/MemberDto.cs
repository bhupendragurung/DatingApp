namespace Api.Features.Members;

public sealed record MemberDto(
    Guid Id,
    string UserName,
    string KnownAs,
    int Age,
    string Gender,
    string City,
    string Country,
    DateTime LastActive
    );