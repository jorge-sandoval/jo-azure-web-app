using Azure.Storage.Queues;
using System.Text.Json;
using jo_azure_web_app.Data;
using jo_azure_web_app.Data.Configuration;
using Microsoft.Extensions.Options;

namespace jo_azure_web_app.Services
{
    public class QueueService : IQueueService
    {
        private readonly QueueServiceClient _queueServiceClient;
        public QueueService(IOptions<ConnectionStringsSettings> connectionStringOptions)
        {
            var connectionString = connectionStringOptions.Value.AzureStorageConnection;
            _queueServiceClient = new QueueServiceClient(
                connectionString, new QueueClientOptions{ MessageEncoding = QueueMessageEncoding.Base64 }
            );
        }

        public async Task SendEmailAsync(string queueName, EmailMessage message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
            var messageJson = JsonSerializer.Serialize(message);
            await queueClient.SendMessageAsync(messageJson);
        }
    }
}
