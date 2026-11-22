using Api.Features.Photos.UploadPhoto;
using Api.Tests.Fakes;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Features.Photos;

public sealed class UploadPhotoHandlerTests
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static UploadPhotoCommand CreateCommand() =>
        new(new MemoryStream([1, 2, 3]), "image/jpeg", 3) { UserId = UserId };

    [Fact]
    public async Task Handle_MakesFirstPhotoMain()
    {
        await using var db = CreateDb();
        var handler = new UploadPhotoHandler(db, new FakePhotoStorage());

        var result = await handler.Handle(CreateCommand(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsMain);
    }

    [Fact]
    public async Task Handle_DoesNotMakeLaterPhotosMain()
    {
        await using var db = CreateDb();
        db.Photos.Add(new Photo
        {
            Url = "https://fake.blob/photos/old.jpg",
            BlobName = "old.jpg",
            IsMain = true,
            AppUserId = UserId
        });
        await db.SaveChangesAsync();
        var handler = new UploadPhotoHandler(db, new FakePhotoStorage());

        var result = await handler.Handle(CreateCommand(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.Value.IsMain);
        Assert.Equal(1, await db.Photos.CountAsync(p => p.IsMain));
    }
}