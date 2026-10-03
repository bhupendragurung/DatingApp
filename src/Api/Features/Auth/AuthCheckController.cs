using Api.Common.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Auth;

public sealed record AuthCheckDto(string UserName, IReadOnlyList<string> Roles);

[ApiController]
[Route("api/auth-check")]
[Authorize]
public sealed class AuthCheckController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<AuthCheckDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<AuthCheckDto> Get() => Ok(ToDto());

    [HttpGet("moderator")]
    [Authorize(Policy = Policies.RequireModerator)]
    [ProducesResponseType<AuthCheckDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<AuthCheckDto> Moderator() => Ok(ToDto());

    [HttpGet("admin")]
    [Authorize(Policy = Policies.RequireAdmin)]
    [ProducesResponseType<AuthCheckDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<AuthCheckDto> Admin() => Ok(ToDto());

    private AuthCheckDto ToDto() =>
        new(User.Identity!.Name!, User.FindAll("role").Select(c => c.Value).ToList());
}