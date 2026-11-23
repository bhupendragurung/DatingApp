using Api.Features.Photos.SetMainPhoto;
using Domain.Common;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Features.Photos;

public sealed class SetMainPhotoHandlerTests
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
    public async Task Handle_MovesMainToChosenPhoto()
    {
        await using var db = CreateDb();
        var oldMain = CreatePhoto(UserId, isMain: true);
        var chosen = CreatePhoto(UserId, isMain: false);
        db.Photos.AddRange(oldMain, chosen);
        await db.SaveChangesAsync();

        var command = new SetMainPhotoCommand(chosen.Id) { UserId = UserId };
        var result = await new SetMainPhotoHandler(db).Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var main = await db.Photos.SingleAsync(p => p.IsMain);
        Assert.Equal(chosen.Id, main.Id);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_ForAnotherUsersPhoto()
    {
        await using var db = CreateDb();
        var otherUsersPhoto = CreatePhoto(Guid.NewGuid(), isMain: false);
        db.Photos.Add(otherUsersPhoto);
        await db.SaveChangesAsync();

        var command = new SetMainPhotoCommand(otherUsersPhoto.Id) { UserId = UserId };
        var result = await new SetMainPhotoHandler(db).Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.False(otherUsersPhoto.IsMain);
    }
}