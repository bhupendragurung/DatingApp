namespace Infrastructure.Storage;

public interface IPhotoStorage
{
    Task<StoredPhoto> UploadAsync(
        Stream content,
        string fileExtension,
        string contentType,
        CancellationToken cancellationToken);

    Task DeleteAsync(string blobName, CancellationToken cancellationToken);
}
public record StoredPhoto(string BlobName, string Url);