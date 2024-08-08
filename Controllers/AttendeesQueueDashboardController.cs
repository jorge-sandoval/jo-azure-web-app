using Azure.Storage.Queues.Models;
using jo_azure_web_app.Data;
using jo_azure_web_app.Data.Configuration;
using jo_azure_web_app.Services;
using jo_azure_web_app.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace jo_azure_web_app.Controllers
{
    public class AttendeesQueueDashboardController : Controller
    {
        private readonly IQueueService _queueService;
        private readonly string _queueName;
        private static QueueMessage? _currentMessage;

        public AttendeesQueueDashboardController(
            IOptions<AzureStorageSettings> storageOptions,
            IQueueService queueService
        )
        {
            _queueName = storageOptions.Value.Queues.AttendeesEmailsQueueName;
            _queueService = queueService;
        }

        public async Task<IActionResult> Index()
        {
            var totalMessages = await _queueService.GetMessageCountAsync(_queueName);

            var viewModel = new AttendeesQueueDashboardViewModel
            {
                CurrentMessage = _currentMessage,
                TotalMessages = totalMessages
            };

            if (viewModel.CurrentMessage != null)
            {
                viewModel.EmailMessage = _queueService.DecodeMessage<EmailMessage>(viewModel.CurrentMessage);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetNextMessage()
        {
            _currentMessage = await _queueService.GetNextMessageAsync(_queueName, TimeSpan.FromMinutes(1));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(string messageId, string popReceipt)
        {
            if (_currentMessage != null && _currentMessage.MessageId == messageId && _currentMessage.PopReceipt == popReceipt)
            {
                await _queueService.DeleteMessageAsync(_queueName, messageId, popReceipt);
                _currentMessage = null;
            }
            return RedirectToAction("Index");
        }
    }
}
