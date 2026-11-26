using Api.Common;
using Api.Common.Auth;
using Api.Common.Paging;
using Api.Features.Likes.GetLikes;
using Api.Features.Likes.LikeUser;
using Api.Features.Members;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Likes;

[ApiController]
[Route("api/likes")]
[Authorize]
public sealed class LikesController(ISender sender) : ControllerBase
{
    [HttpPost("{targetUserId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Like(
        Guid targetUserId,
        CancellationToken cancellationToken)
    {
        var command = new LikeUserCommand(targetUserId)
        {
            UserId = User.GetUserId()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : this.ToProblem(result.Error);
    }
    [HttpGet]
    [ProducesResponseType<PagedResult<MemberDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<MemberDto>>> GetLikes(
        [FromQuery] GetLikesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            query with { CurrentUserId = User.GetUserId() },
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : this.ToProblem(result.Error);
    }
}