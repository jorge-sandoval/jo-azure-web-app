using jo_azure_web_app.Data;
using jo_azure_web_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jo_azure_web_app.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly IAttendeesService _attendeesService;

        public AttendeesController(IAttendeesService attendeesService)
        {
            _attendeesService = attendeesService;
        }

        public ActionResult Index()
        {
            var attendees = _attendeesService.GetAttendees();
            return View(attendees);
        }

        public async Task<ActionResult> Details(string industry, string id)
        {
            var attendee = await _attendeesService.GetAttendee(industry, id);
            return View(attendee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Attendee attendee)
        {
            try
            {
                attendee.PartitionKey = attendee.Industry;
                attendee.RowKey = Guid.NewGuid().ToString();

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
        public async Task<ActionResult> Edit(Attendee attendee)
        {
            try
            {
                attendee.PartitionKey = attendee.Industry;
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
            return View(attendee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAttendee(string industry, string id)
        {
            try
            {
                await _attendeesService.DeleteAtrendee(industry, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
