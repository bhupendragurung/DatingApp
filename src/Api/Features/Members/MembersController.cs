using Api.Common;
using Api.Common.Auth;
using Api.Common.Paging;
using Api.Features.Members.GetMemberById;
using Api.Features.Members.GetMembers;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Members;

[ApiController]
[Route("api/members")]
[Authorize]
public sealed class MembersController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PagedResult<MemberDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<MemberDto>>> GetMembers(
        [FromQuery] GetMembersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            query with { CurrentUserId = User.GetUserId() },
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : this.ToProblem(result.Error);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<MemberDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MemberDetailDto>> GetMember(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMemberByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : this.ToProblem(result.Error);
    }
}