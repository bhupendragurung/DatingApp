using Api.Common;
using Api.Features.Auth.Register;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Auth;

[ApiController]
[Route("api/account")]
public sealed class AccountController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Register(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : this.ToProblem(result.Error);
    }
}