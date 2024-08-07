using jo_azure_web_app.Data;
using jo_azure_web_app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace jo_azure_web_app.Pages.Engineers
{
    public class UpsertModel : PageModel
    {
        private readonly IEngineerService _engineerService;

        public UpsertModel(IEngineerService engineerService)
        {
            _engineerService = engineerService;
        }

        [BindProperty]
        public Engineer Engineer { get; set; } = default!;

        public string PageTitle { get; set; }
        public string ButtonText { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                Engineer = new Engineer();
                PageTitle = "Create Engineer";
                ButtonText = "Create";
            }
            else
            {
                Engineer = await _engineerService.GetEnginnerById(id.ToString(), id.ToString());
                if (Engineer == null)
                {
                    return NotFound();
                }
                PageTitle = "Edit Engineer";
                ButtonText = "Update";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Engineer.id == Guid.Empty)
            {
                Engineer.id = Guid.NewGuid();
                await _engineerService.AddEnginner(Engineer);
            }
            else
            {
                await _engineerService.UpdateEnginner(Engineer);
            }

            return RedirectToPage("./Index");
        }
    }
}
