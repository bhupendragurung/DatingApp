using Domain.Common;
using Mediator;

namespace Api.Features.Likes.LikeUser;

public sealed record LikeUserCommand(Guid TargetUserId) : ICommand<Result>
{
    public Guid UserId { get; init; }
}