using Api.Features.Photos.DeletePhoto;
using Api.Tests.Fakes;
using Domain.Common;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Features.Photos;

public sealed class DeletePhotoHandlerTests
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static Photo CreatePhoto(Guid userId, bool isMain)
    {
        var blobName = $"{Guid.NewGuid()}.jpg";
        return new Photo
        {
            Id = Guid.NewGuid(),
            Url = $"https://fake.blob/photos/{blobName}",
            BlobName = blobName,
            IsMain = isMain,
            AppUserId = userId
        };
    }

    [Fact]
    public async Task Handle_RefusesToDeleteMainPhoto()
    {
        await using var db = CreateDb();
        var main = CreatePhoto(UserId, isMain: true);
        db.Photos.Add(main);
        await db.SaveChangesAsync();
        var storage = new FakePhotoStorage();

        var command = new DeletePhotoCommand(main.Id) { UserId = UserId };
        var result = await new DeletePhotoHandler(db, storage).Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        Assert.Equal(1, await db.Photos.CountAsync());
        Assert.Empty(storage.DeletedBlobNames);
    }

    [Fact]
    public async Task Handle_DeletesNonMainPhoto_FromDatabaseAndStorage()
    {
        await using var db = CreateDb();
        var main = CreatePhoto(UserId, isMain: true);
        var extra = CreatePhoto(UserId, isMain: false);
        db.Photos.AddRange(main, extra);
        await db.SaveChangesAsync();
        var storage = new FakePhotoStorage();

        var command = new DeletePhotoCommand(extra.Id) { UserId = UserId };
        var result = await new DeletePhotoHandler(db, storage).Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var remaining = await db.Photos.SingleAsync();
        Assert.Equal(main.Id, remaining.Id);
        Assert.Equal([extra.BlobName], storage.DeletedBlobNames);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_ForAnotherUsersPhoto()
    {
        await using var db = CreateDb();
        var otherUsersPhoto = CreatePhoto(Guid.NewGuid(), isMain: false);
        db.Photos.Add(otherUsersPhoto);
        await db.SaveChangesAsync();
        var storage = new FakePhotoStorage();

        var command = new DeletePhotoCommand(otherUsersPhoto.Id) { UserId = UserId };
        var result = await new DeletePhotoHandler(db, storage).Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal(1, await db.Photos.CountAsync());
        Assert.Empty(storage.DeletedBlobNames);
    }
}
