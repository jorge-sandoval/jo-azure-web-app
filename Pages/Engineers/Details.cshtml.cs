using jo_azure_web_app.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace jo_azure_web_app.Pages.Engineers
{
    public class DetailsModel : PageModel
    {
        private readonly IEngineerService _engineerService;

        public DetailsModel(IEngineerService engineerService)
        {
            _engineerService = engineerService;
        }

        public Engineer Engineer { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Engineer = await _engineerService.GetEnginnerById(id, id);
            if (Engineer == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
