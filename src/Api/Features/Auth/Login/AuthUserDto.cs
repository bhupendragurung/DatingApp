namespace Api.Features.Auth.Login;

public sealed record AuthUserDto(Guid Id, string UserName, string KnownAs, string Token);