using Domain.Common;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Storage;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Photos.UploadPhoto;

public sealed class UploadPhotoHandler(AppDbContext db, IPhotoStorage storage)
    : ICommandHandler<UploadPhotoCommand, Result<PhotoDto>>
{
    public async ValueTask<Result<PhotoDto>> Handle(
        UploadPhotoCommand command,
        CancellationToken cancellationToken)
    {
        var extension = command.ContentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            _ => ".webp"
        };

        var hasPhotos = await db.Photos
            .AnyAsync(p => p.AppUserId == command.UserId, cancellationToken);

        var stored = await storage.UploadAsync(
            command.Content,
            extension,
            command.ContentType,
            cancellationToken);

        var photo = new Photo
        {
            Url = stored.Url,
            BlobName = stored.BlobName,
            IsMain = !hasPhotos,
            AppUserId = command.UserId
        };

        db.Photos.Add(photo);
        await db.SaveChangesAsync(cancellationToken);

        return Result<PhotoDto>.Success(new PhotoDto(photo.Id, photo.Url, photo.IsMain));
    }
}