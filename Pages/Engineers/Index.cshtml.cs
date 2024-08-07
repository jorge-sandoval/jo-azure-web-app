using jo_azure_web_app.Data;
using jo_azure_web_app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace jo_azure_web_app.Pages.Engineers
{
    public class IndexModel : PageModel
    {
        private readonly IEngineerService _engineerService;

        public IndexModel(IEngineerService engineerService)
        {
            _engineerService = engineerService;
        }

        public IList<Engineer> Engineers { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Engineers = await _engineerService.GetEngineersAsync();
        }
    }
}
