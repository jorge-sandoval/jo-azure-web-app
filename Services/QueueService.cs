using Azure.Storage.Queues;
using System.Text.Json;
using jo_azure_web_app.Data;
using jo_azure_web_app.Data.Configuration;
using Microsoft.Extensions.Options;
using Azure.Storage.Queues.Models;
using System.Text;

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

        public async Task SendMessageAsync<T>(string queueName, T message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
            var messageJson = JsonSerializer.Serialize<T>(message);
            var messageBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageJson));
            await queueClient.SendMessageAsync(messageBase64);
        }


        public async Task<int> GetMessageCountAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var properties = await queueClient.GetPropertiesAsync();
            return properties.Value.ApproximateMessagesCount;
        }

        public async Task<QueueMessage?> GetNextMessageAsync(string queueName)
        {
            return await GetNextMessageAsync(queueName, null);
        }

        public async Task<QueueMessage?> GetNextMessageAsync(string queueName, TimeSpan? visibilityTimeOut)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var messages = await queueClient.ReceiveMessagesAsync(1, visibilityTimeOut);
            return messages.Value.FirstOrDefault();
        }

        public T? DecodeMessage<T>(QueueMessage? queueMessage)
        {
            if(queueMessage == null)
            {
                return default(T);

            }

            var decodedMessage = Encoding.UTF8.GetString(Convert.FromBase64String(queueMessage.MessageText));
            return JsonSerializer.Deserialize<T>(decodedMessage);
        }

        public async Task DeleteMessageAsync(string queueName, string messageId, string popReceipt)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }
}
