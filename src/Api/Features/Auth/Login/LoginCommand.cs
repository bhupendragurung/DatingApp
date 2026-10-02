using Domain.Common;
using Mediator;

namespace Api.Features.Auth.Login;

public sealed record LoginCommand(string UserName, string Password) : IRequest<Result<AuthUserDto>>;