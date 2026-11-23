using Infrastructure.Storage;

namespace Api.Tests.Fakes;

public sealed class FakePhotoStorage : IPhotoStorage
{
    public List<string> DeletedBlobNames { get; } = [];

    public Task<StoredPhoto> UploadAsync(
        Stream content,
        string fileExtension,
        string contentType,
        CancellationToken cancellationToken)
    {
        var blobName = $"{Guid.NewGuid()}{fileExtension}";
        var stored = new StoredPhoto(blobName, $"https://fake.blob/photos/{blobName}");
        return Task.FromResult(stored);
    }

    public Task DeleteAsync(string blobName, CancellationToken cancellationToken)
    {
        DeletedBlobNames.Add(blobName);
        return Task.CompletedTask;
    }
}
