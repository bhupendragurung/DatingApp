using Api.Features.Auth;
using Domain.Common;
using Mediator;

namespace Api.Features.Auth.Register;

public sealed record RegisterCommand(
    string UserName,
    string Password,
    string KnownAs,
    string Gender,
    DateOnly DateOfBirth,
    string City,
    string Country) :  IRequest<Result<UserDto>>;