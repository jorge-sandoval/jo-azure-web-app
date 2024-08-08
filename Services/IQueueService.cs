using Azure.Storage.Queues.Models;
using jo_azure_web_app.Data;

namespace jo_azure_web_app.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(string queueName, T message);
        Task<int> GetMessageCountAsync(string queueName);
        Task<QueueMessage?> GetNextMessageAsync(string queueName, TimeSpan? visibilityTimeOut);
        Task<QueueMessage?> GetNextMessageAsync(string queueName);
        Task DeleteMessageAsync(string queueName, string messageId, string popReceipt);
        T? DecodeMessage<T>(QueueMessage? queueMessage);
    }
}