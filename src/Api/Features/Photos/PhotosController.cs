using Api.Common;
using Api.Common.Auth;
using Api.Features.Photos.DeletePhoto;
using Api.Features.Photos.SetMainPhoto;
using Api.Features.Photos.UploadPhoto;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Photos;

[ApiController]
[Route("api/photos")]
[Authorize]
public sealed class PhotosController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<PhotoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PhotoDto>> Upload(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();

        var command = new UploadPhotoCommand(stream, file.ContentType, file.Length)
        {
            UserId = User.GetUserId()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result.Value)
            : this.ToProblem(result.Error);
    }
    
    [HttpPut("{photoId:guid}/main")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SetMain(
        Guid photoId,
        CancellationToken cancellationToken)
    {
        var command = new SetMainPhotoCommand(photoId)
        {
            UserId = User.GetUserId()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : this.ToProblem(result.Error);
    }
    [HttpDelete("{photoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(
        Guid photoId,
        CancellationToken cancellationToken)
    {
        var command = new DeletePhotoCommand(photoId)
        {
            UserId = User.GetUserId()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : this.ToProblem(result.Error);
    }
}