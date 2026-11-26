using Domain.Common;
using Infrastructure;
using Infrastructure.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Likes.LikeUser;

public sealed class LikeUserHandler(AppDbContext db)
    : ICommandHandler<LikeUserCommand, Result>
{
    private static readonly Error UserNotFound =
        Error.NotFound("Likes.UserNotFound", "The user was not found.");

    private static readonly Error AlreadyLiked =
        Error.Conflict("Likes.AlreadyLiked", "You have already liked this user.");

    public async ValueTask<Result> Handle(
        LikeUserCommand command,
        CancellationToken cancellationToken)
    {
        var targetExists = await db.Users
            .AnyAsync(u => u.Id == command.TargetUserId, cancellationToken);
        if (!targetExists)
        {
            return Result.Failure(UserNotFound);
        }

        var alreadyLiked = await db.Likes
            .AnyAsync(l => l.SourceUserId == command.UserId
                        && l.TargetUserId == command.TargetUserId,
                      cancellationToken);
        if (alreadyLiked)
        {
            return Result.Failure(AlreadyLiked);
        }

        db.Likes.Add(new UserLike
        {
            SourceUserId = command.UserId,
            TargetUserId = command.TargetUserId
        });
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}