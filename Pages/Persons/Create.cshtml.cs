using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using jo_azure_web_app.Data;

namespace jo_azure_web_app.Pages_Persons
{
    public class CreateModel : PageModel
    {
        private readonly jo_azure_web_app.Data.AppDbContext _context;

        public CreateModel(jo_azure_web_app.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Person Person { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.persons.Add(Person);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
