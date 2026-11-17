using Domain.Common;
using Infrastructure;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Photos.SetMainPhoto;

public sealed class SetMainPhotoHandler(AppDbContext db)
    : ICommandHandler<SetMainPhotoCommand, Result>
{
    private static readonly Error PhotoNotFound =
        Error.NotFound("Photos.NotFound", "The photo was not found.");

    public async ValueTask<Result> Handle(
        SetMainPhotoCommand command,
        CancellationToken cancellationToken)
    {
        var photos = await db.Photos
            .Where(p => p.AppUserId == command.UserId)
            .ToListAsync(cancellationToken);

        var photo = photos.FirstOrDefault(p => p.Id == command.PhotoId);
        if (photo is null)
        {
            return Result.Failure(PhotoNotFound);
        }

        if (photo.IsMain)
        {
            return Result.Success();
        }

        foreach (var p in photos.Where(p => p.IsMain))
        {
            p.IsMain = false;
        }

        photo.IsMain = true;
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}