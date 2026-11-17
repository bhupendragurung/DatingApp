using Domain.Common;
using Infrastructure;
using Infrastructure.Storage;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Photos.DeletePhoto;

public sealed class DeletePhotoHandler(AppDbContext db, IPhotoStorage storage)
    : ICommandHandler<DeletePhotoCommand, Result>
{
    private static readonly Error PhotoNotFound =
        Error.NotFound("Photos.NotFound", "The photo was not found.");

    private static readonly Error CannotDeleteMain =
        Error.Conflict("Photos.CannotDeleteMain", "You cannot delete your main photo.");

    public async ValueTask<Result> Handle(
        DeletePhotoCommand command,
        CancellationToken cancellationToken)
    {
        var photo = await db.Photos.FirstOrDefaultAsync(
            p => p.Id == command.PhotoId && p.AppUserId == command.UserId,
            cancellationToken);

        if (photo is null)
        {
            return Result.Failure(PhotoNotFound);
        }

        if (photo.IsMain)
        {
            return Result.Failure(CannotDeleteMain);
        }

        db.Photos.Remove(photo);
        await db.SaveChangesAsync(cancellationToken);

        await storage.DeleteAsync(photo.BlobName, cancellationToken);

        return Result.Success();
    }
}