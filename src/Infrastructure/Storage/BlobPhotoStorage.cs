using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Infrastructure.Storage;

public class BlobPhotoStorage(BlobContainerClient container) : IPhotoStorage
{
    public async Task<StoredPhoto> UploadAsync(
        Stream content,
        string fileExtension,
        string contentType,
        CancellationToken cancellationToken)
    {
        var blobName = $"{Guid.NewGuid()}{fileExtension}";
        var blob = container.GetBlobClient(blobName);

        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blob.UploadAsync(content, options, cancellationToken);

        return new StoredPhoto(blobName, blob.Uri.ToString());
    }

    public async Task DeleteAsync(string blobName, CancellationToken cancellationToken)
    {
        var blob = container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}