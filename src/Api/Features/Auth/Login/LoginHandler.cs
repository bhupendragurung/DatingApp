using Api.Common.Auth;
using Domain.Common;
using Infrastructure.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace Api.Features.Auth.Login;

public sealed class LoginHandler(UserManager<AppUser> userManager, TokenService tokenService)
    : IRequestHandler<LoginCommand, Result<AuthUserDto>>
{
    private static readonly Error InvalidCredentials =
        Error.Unauthorized("Auth.InvalidCredentials", "Invalid username or password.");

    public async ValueTask<Result<AuthUserDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(command.UserName);
        if (user is null || !await userManager.CheckPasswordAsync(user, command.Password))
        {
            return Result<AuthUserDto>.Failure(InvalidCredentials);
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.CreateToken(user, roles);

        return Result<AuthUserDto>.Success(new AuthUserDto(user.Id, user.UserName!, user.KnownAs, token));
    }
}