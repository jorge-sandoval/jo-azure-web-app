using jo_azure_web_app.Data;
using jo_azure_web_app.Data.Configuration;
using jo_azure_web_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace jo_azure_web_app.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly string _imagesContainerName;
        private readonly IAttendeesService _attendeesService;
        private readonly IBlobStorageService _blobStorageService;

        public AttendeesController(
            IOptions<AzureStorageSettings> storageOptions,
            IAttendeesService attendeesService,
            IBlobStorageService blobStorageService
        )
        {
            _imagesContainerName = storageOptions.Value.Containers.AttendeesImagesContainerName;
            _attendeesService = attendeesService;
            _blobStorageService = blobStorageService;
        }

        public async Task<ActionResult> Index()
        {
            var attendees = _attendeesService.GetAttendees();
            
            var tasks = attendees
                .Where(attendee => !string.IsNullOrEmpty(attendee.ImageName))
                .Select(async attendee =>
                {
                    attendee.ImageName = await _blobStorageService.GetBlobUrlAsync(_imagesContainerName, attendee.ImageName);
                }).ToArray();
            await Task.WhenAll(tasks);

            return View(attendees);
        }

        public async Task<ActionResult> Details(string industry, string id)
        {
            var attendee = await _attendeesService.GetAttendee(industry, id);
            if (!string.IsNullOrEmpty(attendee.ImageName))
            {
                attendee.ImageName = await _blobStorageService.GetBlobUrlAsync(_imagesContainerName, attendee.ImageName);
            }
            return View(attendee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Attendee attendee, IFormFile formFile)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                attendee.PartitionKey = attendee.Industry;
                attendee.RowKey = id;

                if (formFile != null && formFile.Length > 0)
                {
                    attendee.ImageName = await _blobStorageService.UploadBlobAsync(_imagesContainerName, id, formFile);
                }
                else
                {
                    attendee.ImageName = "default.jpg";
                }

                await _attendeesService.UpsertAtrendee(attendee);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> Edit(string industry, string id)
        {
            var attendee = await _attendeesService.GetAttendee(industry, id);

            return View(attendee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Attendee attendee, IFormFile formFile)
        {
            try
            {
                attendee.PartitionKey = attendee.Industry;

                if (formFile != null && formFile.Length > 0)
                {
                    attendee.ImageName = await _blobStorageService.UploadBlobAsync(_imagesContainerName, attendee.RowKey, formFile, attendee.ImageName);
                }

                await _attendeesService.UpsertAtrendee(attendee);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(string industry, string id)
        {
            var attendee = await _attendeesService.GetAttendee(industry, id);

            if (!string.IsNullOrEmpty(attendee.ImageName))
            {
                attendee.ImageName = await _blobStorageService.GetBlobUrlAsync(_imagesContainerName, attendee.ImageName);
            }

            return View(attendee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAttendee(string industry, string id)
        {
            try
            {
                var attendee = await _attendeesService.GetAttendee(industry, id);
                await _attendeesService.DeleteAtrendee(industry, id);
                await _blobStorageService.DeleteBlobAsync(_imagesContainerName, attendee.ImageName);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
