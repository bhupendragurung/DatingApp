using Domain.Common;
using Mediator;

namespace Api.Features.Photos.SetMainPhoto;

public sealed record SetMainPhotoCommand(Guid PhotoId) : ICommand<Result>
{
    public Guid UserId { get; init; }
}