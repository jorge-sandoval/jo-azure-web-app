using Azure.Storage.Queues.Models;
using jo_azure_web_app.Data;

namespace jo_azure_web_app.ViewModels
{
    public class AttendeesQueueDashboardViewModel
    {
        public QueueMessage? CurrentMessage { get; set; }
        public EmailMessage? EmailMessage { get; set; }
        public int TotalMessages { get; set; }
    }
}
