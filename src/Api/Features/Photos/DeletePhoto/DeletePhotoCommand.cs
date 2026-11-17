using Domain.Common;
using Mediator;

namespace Api.Features.Photos.DeletePhoto;

public sealed record DeletePhotoCommand(Guid PhotoId) : ICommand<Result>
{
    public Guid UserId { get; init; }
}