using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using jo_azure_web_app.Data.Configuration;
using Microsoft.Extensions.Options;

namespace jo_azure_web_app.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IOptions<ConnectionStringsSettings> connectionStringOptions)
        {
            var connectionString = connectionStringOptions.Value.AzureStorageConnection;
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        private BlobContainerClient GetContainerClient(string containerName)
        {
            return _blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            var containerClient = GetContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, overwrite: true);
        }

        public async Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile formFile)
        {
            return await UploadBlobAsync(containerName, blobName, formFile, "");
        }

        public async Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile formFile, string originalBlobName)
        {
            if (!string.IsNullOrEmpty(originalBlobName))
            {
                await DeleteBlobAsync(containerName, originalBlobName);
            }
            
            var renamed = $"{blobName}{Path.GetExtension(formFile.FileName)}";
            using (var stream = formFile.OpenReadStream())
            {
                await UploadBlobAsync(containerName, renamed, stream);
            }
            return renamed;
        }

        public async Task<string> GetBlobUrlAsync(string containerName, string blobName)
        {
            var containerClient = GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();

            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            return blobClient.GenerateSasUri(blobSasBuilder).ToString();
        }

        public async Task<Stream> GetBlobAsync(string containerName, string blobName)
        {
            var containerClient = GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var containerClient = GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}
