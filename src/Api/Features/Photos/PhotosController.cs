using Api.Common;
using Api.Common.Auth;
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
}