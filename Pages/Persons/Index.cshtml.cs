using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using jo_azure_web_app.Data;

namespace jo_azure_web_app.Pages_Persons
{
    public class IndexModel : PageModel
    {
        private readonly jo_azure_web_app.Data.AppDbContext _context;

        public IndexModel(jo_azure_web_app.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Person = await _context.persons.ToListAsync();
        }
    }
}
