using Microsoft.AspNetCore.Mvc;

namespace jo_azure_web_app.Controllers
{
    public class StorageController : Controller
    {
        public IActionResult BlobStorage()
        {
            return View();
        }
    }
}
