using jo_azure_web_app.Data;

namespace jo_azure_web_app.Services
{
    public interface IQueueService
    {
        Task SendEmailAsync(string queueName, EmailMessage message);
    }
}