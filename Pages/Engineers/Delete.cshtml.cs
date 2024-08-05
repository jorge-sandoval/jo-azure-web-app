using jo_azure_web_app.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace jo_azure_web_app.Pages.Engineers
{
    public class DeleteModel : PageModel
    {
        private readonly IEngineerService _engineerService;

        public DeleteModel(IEngineerService engineerService)
        {
            _engineerService = engineerService;
        }

        [BindProperty]
        public Engineer Engineer { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Engineer = await _engineerService.GetEnginnerById(id.ToString(), id.ToString());

            if (Engineer == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _engineerService.DeleteEnginner(Engineer.id.ToString(), Engineer.id.ToString());

            return RedirectToPage("./Index");
        }
    }
}
