using Domain.Common;
using Mediator;

namespace Api.Features.Photos.UploadPhoto;

public sealed record UploadPhotoCommand(
    Stream Content,
    string ContentType,
    long Length) : ICommand<Result<PhotoDto>>
{
    public Guid UserId { get; init; }
}