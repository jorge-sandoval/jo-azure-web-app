
namespace jo_azure_web_app.Services
{
    public interface IBlobStorageService
    {
        Task DeleteBlobAsync(string containerName, string blobName);
        Task<Stream> GetBlobAsync(string containerName, string blobName);
        Task<string> GetBlobUrlAsync(string containerName, string blobName);
        Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile formFile, string originalBlobName);
        Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile formFile);
        Task UploadBlobAsync(string containerName, string blobName, Stream content);
    }
}